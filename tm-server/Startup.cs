using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Threading.Tasks;
using tm_server.Middlewares;
using tm_server.Models;

namespace tm_server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<TMContext>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "tm_server", Version = "v1" });
            });
            services.AddCors();

            services.AddAuthentication();
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "Merlin";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.Events.OnRedirectToAccessDenied = new Func<RedirectContext<CookieAuthenticationOptions>, Task>(context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return context.Response.CompleteAsync();
                });
            });

            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<TMContext>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TMContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                context.Database.EnsureDeleted();
                context.Database.Migrate();
                AddTestData(context, userManager, roleManager);
            }

            if (true || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "tm_server v1"));
            }
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseMiddleware<RouteLog>();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    /*context.Request.Path = "/index.html";*/
                    //await next();
                    await context.Response.CompleteAsync();
                }
            });

            AddAngularStatics(app, env, "Views");




            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddTestData(TMContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            roleManager.CreateAsync(new IdentityRole { Name = "Administrator" }).Wait();
            roleManager.CreateAsync(new IdentityRole { Name = "User" }).Wait();

            var currUsr = new AppUser { UserName = "admin" };
            userManager.CreateAsync(currUsr, "Admin@123").Wait();
            userManager.AddToRolesAsync(currUsr, new[] { "Administrator", "User" }).Wait();

            currUsr = new AppUser { UserName = "user" };
            userManager.CreateAsync(currUsr, "User@123").Wait();
            userManager.AddToRolesAsync(currUsr, new[] { "User" }).Wait();

            context.Countries.AddRange(new[] {
                new Country { Name = "VietNam" },
                new Country { Name = "China" },
                new Country { Name = "England"},
            });
            context.SaveChanges();
        }

        private static void AddAngularStatics(IApplicationBuilder app, IWebHostEnvironment env, string StaticPath)
        {
            var CombinedPath = Path.Combine(env.ContentRootPath, StaticPath);
            if (!Directory.Exists(CombinedPath))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("warn: ");
                Console.ResetColor();
                Console.WriteLine("Statics folder not found, skipping serve statics folder...");
                Console.WriteLine("      To use statics folder, create Views folder in content root directory");
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("info: ");
                Console.ResetColor();
                Console.WriteLine("Statics folder found, serving statics folder...");
            };

            var FileProvider = new PhysicalFileProvider(CombinedPath);
            /*            var options = new DefaultFilesOptions();
                        options.DefaultFileNames.Clear();
                        options.DefaultFileNames.Add("index.html");
                        options.FileProvider = FileProvider;
                        app.UseDefaultFiles(options);
                        app.UseStaticFiles(new StaticFileOptions
                        {
                            FileProvider = FileProvider,
                            RequestPath = ""
                        });*/

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = FileProvider,
                EnableDirectoryBrowsing = true,
                RequestPath = ""
            });
        }
    }
}

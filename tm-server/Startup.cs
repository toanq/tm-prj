using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using tm_server.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using tm_server.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using tm_server.Authorization;
using tm_server.Services;

namespace tm_server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private readonly string corsWhitelist = "corsWhitelist";

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
            services.AddCors(options => options.AddPolicy(
                name: this.corsWhitelist,
                builder =>
                {
                    builder.WithOrigins("http://nqtoan.ddns.net");
                }
            ));

            string keyString = Configuration.GetSection("AppSettings").GetValue<string>("SecretKey");
            byte[] key = Encoding.ASCII.GetBytes(keyString);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IJwtUtils, JwtUtils>();
            services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<TMContext>();
                var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
                context.Database.EnsureDeleted();
                context.Database.Migrate();
                AddTestData(context, userService);
            }

            if (true || env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "tm_server v1"));
            }
            app.UseCors(this.corsWhitelist);
            
            app.UseMiddleware<RouteLog>();
            app.UseMiddleware<JwtMiddleware>();

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    /*context.Request.Path = "/index.html";*/
                    //await next();
                    await context.Response.WriteAsync("");
                }
            });

            AddAngularStatics(app, env, "Views");

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void AddTestData(TMContext context, IUserService userService)
        {
            var CreateUser = userService.Create;
            context.Users.AddRange(new[]
            {
                CreateUser("admin", "admin", Role.Admin),
                CreateUser("user", "user", Role.User),
                CreateUser("test", "test", Role.User)
            });

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
            else {
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

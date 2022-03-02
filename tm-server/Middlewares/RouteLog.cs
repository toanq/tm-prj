using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace tm_server.Middlewares
{
    public class RouteLog
    {
        private readonly RequestDelegate _next;
        public RouteLog(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
            context.Response.Headers.Add("Test-middleware", "success");
            await _next(context);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Net;
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
            context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
            context.Response.Headers.Add("Test-middleware", "success");
            await _next(context);
        }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace tm_server.Middlewares
{
    public class Test
    {
        private readonly RequestDelegate _next;
        public Test(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
            context.Response.Headers.Add("Test", "success");
            await _next(context);
        }
    }
}

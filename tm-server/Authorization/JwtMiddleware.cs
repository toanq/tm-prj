using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using tm_server.Services;

namespace tm_server.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IJwtUtils jwtUtils, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                context.Items["User"] = userService.GetById(userId);
            }

            await _next(context);
        }
    }
}

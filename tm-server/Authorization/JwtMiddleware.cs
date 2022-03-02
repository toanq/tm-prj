using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;
using tm_server.Services;

namespace tm_server.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserService _userService;
        public JwtMiddleware(RequestDelegate next, IUserService userService)
        {
            _next = next;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context, IJwtUtils jwtUtils)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var userId = jwtUtils.ValidateToken(token);
            if (userId != null)
            {
                context.Items["User"] = _userService.GetUserById(userId);
            }

            await _next(context);
        }
    }
}

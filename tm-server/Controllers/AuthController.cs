using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using tm_server.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tm_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] key;
        public AuthController(
            IConfiguration configuration
        ) {
            _configuration = configuration;

            string keyString = _configuration.GetSection("AppSettings").GetValue<string>("SecretKey");
            key = Encoding.ASCII.GetBytes(keyString);
        }

        // GET: api/auth
        [HttpGet]
        public IActionResult Get([FromHeader] string authorization)
        {
            if(AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                TokenValidationParameters tokenValidationParams = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                //string scheme = headerValue.Scheme;
                string parameter = headerValue.Parameter;
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(parameter, tokenValidationParams, out SecurityToken validatedToken);
                long timeLeft = (long) (validatedToken.ValidTo - System.DateTime.UtcNow).TotalSeconds;
                return Ok(new
                {
                    Message = "Authentication success",
                    TimeLeft = timeLeft
                });
            } else
            {
                return BadRequest();
            }
        }

        // POST api/auth

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] UserLogin user)
        {
            //user.Print();
            if (user.Username == "admin" && user.Password == "admin")
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(30),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                return Ok(new
                {
                    user.Username,
                    user.Password,
                    Token = tokenString
                });
            } else
            {
                return BadRequest(new { Message = "Username or password is incorrect" });
            }
        }
    }
}

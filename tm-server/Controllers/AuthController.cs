using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using tm_server.Models;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tm_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AuthController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/auth
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            if(_signInManager.IsSignedIn(User))
            {
                var currUsr = await _userManager.GetUserAsync(User);
                return Ok(currUsr);
            } else
                return BadRequest(new { Message = "Unauthrozied" });
        }

        // POST api/auth

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] UserLogin user)
        {
            var signIn = await _signInManager.PasswordSignInAsync(user.UserName, user.Password, true, true);
            if (signIn.Succeeded)
            {
                return Ok(new
                {
                    Message = "Login success",
                    user.UserName
                });
            } else
            {
                return BadRequest(new { Message = "Username or password is incorrect"});
            }
        }

        [HttpPost]
        [Route("reg")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegister user)
        {
            var newUser = new AppUser
            {
                UserName = user.UserName,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: true);
                return Ok(user);
            } else
            {
                await _signInManager.SignOutAsync();
                return BadRequest(result.Errors);
            }
            
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }
    }
}

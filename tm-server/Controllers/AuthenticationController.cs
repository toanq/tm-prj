using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;
using tm_server.Models;
using tm_server.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace tm_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtUtils _jwtUtils;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthenticationController(
            IJwtUtils jwtUtils,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager
        )
        {
            _jwtUtils = jwtUtils;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // POST api/auth

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] UserLogin user)
        {
            var currUsr = await _userManager.FindByNameAsync(user.Username);
            
            if (currUsr == null) currUsr = await _userManager.FindByEmailAsync(user.Username);

            if (currUsr != null)
            {
                var signIn = await _signInManager.CheckPasswordSignInAsync(currUsr, user.Password, true);
                if (signIn.Succeeded)
                {
                    var tokenStr = _jwtUtils.Generate(currUsr, out SecurityToken token);

                    return Ok(new { currUsr.UserName, currUsr.Email, token = tokenStr, expire = token.ValidTo });
                }
                if (signIn.IsLockedOut)
                {
                    return BadRequest(new { Message = "User is currently locked out.", LockUntil = currUsr.LockoutEnd });
                }
            }
            return BadRequest(new { Message = "Username or password is incorrect" });
        }

        // GET: api/auth
        [HttpGet]
        [Route("getCurrUsr")]
        public async Task<IActionResult> GetUserInfo()
        {
            var currUsr = await _userManager.GetUserAsync(User);
            return Ok(currUsr);
        }

        [HttpPost]
        [Route("reg")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegister user)
        {
            var currUsr = await _userManager.FindByNameAsync(user.UserName);
            if (currUsr != null) return BadRequest(new {Message = "Access denied"});

            var newUser = new AppUser { UserName = user.UserName, Email = user.Email };

            var result = await _userManager.CreateAsync(newUser, user.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(newUser, isPersistent: true);
                return Ok(user);
            }
            else
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

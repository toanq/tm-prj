using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using tm_server.Models;
using tm_server.Services;

namespace tm_server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly TMContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IPermissionManager _permissionManager;
        public AuthorizationController(
            TMContext context,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<AppUser> signInManager,
            IPermissionManager permissionManager
        )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _permissionManager = permissionManager;

        }

        [HttpPost]
        [Route("grant")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> Grant(string UserID, string PermissionID)
        {
            var currUsr = await _context.Users.FindAsync(UserID);
            if (currUsr == null)
                return NotFound(new { Message = "User not found"});
            
            var currPstn = await _context.Permissions.FindAsync(PermissionID);
            if (currPstn == null)
                return NotFound(new { Message = "Permission not found" });

            _context.UserPermissions.Add(new UserPermission { User = currUsr, Permission = currPstn });
            return Ok();
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> Test()
        {
            var currUsr = await _userManager.GetUserAsync(User);
            if (currUsr == null) return Unauthorized();

            if (await _permissionManager.ValidateAsync(currUsr, "can.read"))
            {
                return Ok(new { Message = "User has can.read permission"});
            }
            return Forbid();
        }
    }
}

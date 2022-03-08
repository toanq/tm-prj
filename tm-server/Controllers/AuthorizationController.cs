using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using tm_server.Authorizations;
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
        private readonly IPermissionManager _permissionManager;
        public AuthorizationController(
            TMContext context,
            UserManager<AppUser> userManager,
            IPermissionManager permissionManager
        )
        {
            _context = context;
            _userManager = userManager;
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
        [Route("list")]
        //[Authorize(Policy = "HasEInUsername")]
        public async Task<IActionResult> Test()
        {
            var currUser = await _userManager.GetUserAsync(User);
            if (currUser == null) return Unauthorized();

            var permissions = await _permissionManager.GetAllOfAsync(currUser);
            return Ok(new {permissions});
        }

        [HttpGet]
        [Route("createRread")]
        [AuthPermission(new[] { "can.create", "can.read" })]
        //[Authorize(Policy = "HasEInUsername")]
        public async Task<IActionResult> Test2()
        {
            var currUser = await _userManager.GetUserAsync(User);
            if (currUser == null) return Unauthorized();

            var permissions = await _permissionManager.GetAllOfAsync(currUser);
            return Ok(new { permissions });
        }

        [HttpGet]
        [Route("createNread")]
        [AuthPermission("can.create")]
        [AuthPermission("can.read")]
        //[Authorize(Policy = "HasEInUsername")]
        public async Task<IActionResult> Test3()
        {
            var currUser = await _userManager.GetUserAsync(User);
            if (currUser == null) return Unauthorized();

            var permissions = await _permissionManager.GetAllOfAsync(currUser);
            return Ok(new { permissions });
        }
    }
}

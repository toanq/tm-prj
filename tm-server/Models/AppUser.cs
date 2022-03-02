using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace tm_server.Models
{
    public class AppUser : IdentityUser
    {
        public Role Role { get; set; }
    }
}

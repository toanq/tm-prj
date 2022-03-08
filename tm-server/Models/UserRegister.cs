using System.ComponentModel.DataAnnotations;

namespace tm_server.Models
{
    public class UserRegister
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
    }
}

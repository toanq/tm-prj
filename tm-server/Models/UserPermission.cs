using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tm_server.Models
{
    public class UserPermission
    {        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}

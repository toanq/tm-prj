using System.ComponentModel.DataAnnotations;

namespace tm_server.Models
{
    public class Player
    {
        [Key]
        public long Id { get; set; }
        public long ClubId { get; set; }
        public long Nation { get; set; }
        public string Name { get; set; }
        public float Height { get; set; }
        public string Position { get; set; }
    }
}

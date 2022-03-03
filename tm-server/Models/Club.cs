using System.ComponentModel.DataAnnotations;

namespace tm_server.Models
{
    public class Club
    {
        [Key]
        public long Id { get; set; }
        public long CountryId { get; set; }
        public string Name { get; set; }
    }
}

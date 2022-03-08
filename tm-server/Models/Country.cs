using System.ComponentModel.DataAnnotations;

namespace tm_server.Models
{
    public class Country
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}

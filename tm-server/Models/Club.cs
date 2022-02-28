using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tm_server.Models
{
    public class Country
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
    }
}

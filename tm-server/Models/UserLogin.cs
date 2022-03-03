using System;
using System.ComponentModel.DataAnnotations;

namespace tm_server.Models
{
    public class UserLogin
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public override string ToString()
        {
            return $"usr: {UserName}/ pwd: {Password}";
        }

        public void Print() => Console.WriteLine(ToString());
    }
}

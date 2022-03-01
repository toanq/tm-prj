using System;
using System.ComponentModel.DataAnnotations;

namespace tm_server.Models
{
    public class UserLogin
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        public override string ToString()
        {
            return $"usr: {Username}/ pwd: {Password}";
        }

        public void Print() => Console.WriteLine(ToString());
    }
}

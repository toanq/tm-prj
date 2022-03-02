using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tm_server.Models;

namespace tm_server.Services
{
    public interface IUserService
    {
        AppUser GetById(string usrId);
        AppUser Create(string username, string password, Role role);
        bool Validate(string usr, string pwd);
    }
    public class UserService : IUserService
    {
        private readonly TMContext _context;
        private readonly IConfiguration _configuration;
        private readonly byte[] key;
        public UserService(TMContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            string keyString = _configuration.GetSection("AppSettings").GetValue<string>("SecretKey");
            key = Encoding.ASCII.GetBytes(keyString);
        }

        public AppUser GetById(string usrId)
        {
            return _context.Users.Find(usrId);
        }

        public AppUser Create(string username, string password, Role role = Role.User)
        {
            
            using var hmac = new System.Security.Cryptography.HMACSHA256(key);
            byte[] abPwd = hmac.ComputeHash(Encoding.ASCII.GetBytes(password));
            var usr = new AppUser
            {
                UserName = username,
                PasswordHash = Convert.ToBase64String(abPwd),
                Role = role
            };
            return usr;
        }

        public bool Validate(string usr, string pwd)
        {
            if (usr == null || pwd == null) return false;
            AppUser tgUsr = (from u in _context.Users where u.UserName == usr select u).FirstOrDefault();
            if (tgUsr == null) return false;

            using var hmac = new System.Security.Cryptography.HMACSHA256(key);
            byte[] chkPwd = hmac.ComputeHash (Encoding.ASCII.GetBytes(pwd));
            byte[] tgPwd = Convert.FromBase64String(tgUsr.PasswordHash);
            for (int ii = 0; ii < chkPwd.Length; ii++)
            {
                if (chkPwd[ii] != tgPwd[ii]) return false;
            }
            return true;
        }
    }
}

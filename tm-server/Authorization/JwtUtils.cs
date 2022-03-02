﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using tm_server.Models;

namespace tm_server.Authorization
{
    public interface IJwtUtils
    {
        string GenerateToken(AppUser usr);
        string? ValidateToken(string token);
    }
    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _config;
        public JwtUtils(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string GenerateToken(AppUser usr)
        {
            string keyString = _config.GetSection("AppSettings").GetValue<string>("SecretKey");
            byte[] key = Encoding.ASCII.GetBytes(keyString);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.Name, usr.UserName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string? ValidateToken(string token)
        {
            if (token == null) return null;
            string keyString = _config.GetSection("AppSettings").GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(keyString);
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                TokenValidationParameters tokenValidationParams = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                tokenHandler.ValidateToken(token, tokenValidationParams, out SecurityToken validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;
                string userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value;
                return userId;
            } catch
            {
                return null;
            };
        }
    }
}
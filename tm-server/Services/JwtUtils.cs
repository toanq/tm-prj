using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tm_server.Models;

namespace tm_server.Services
{
    public interface IJwtUtils
    {
        string Generate(AppUser usr);
        string Generate(AppUser usr, out SecurityToken token);
        SecurityToken Validate(string token);
    }
    public class JwtUtils : IJwtUtils
    {
        private readonly IConfiguration _configuration;
        private readonly byte[] _jwtSecret;
        public JwtUtils(
            IConfiguration configuration
        ) {
            _configuration = configuration;
            _jwtSecret = GetSecretKey();
        }

        public string Generate(AppUser usr)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim(ClaimTypes.Name, usr.UserName),
                            new Claim(ClaimTypes.NameIdentifier, usr.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtSecret), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string Generate(AppUser usr, out SecurityToken token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                            new Claim(ClaimTypes.Name, usr.UserName),
                            new Claim(ClaimTypes.NameIdentifier, usr.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtSecret), SecurityAlgorithms.HmacSha256Signature),
            };
            token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public SecurityToken Validate(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var appSettings = _configuration.GetRequiredSection("AppSettings");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = appSettings.GetValue<bool>("ValidateIssuerSigningKey"),
                IssuerSigningKey = new SymmetricSecurityKey(_jwtSecret),
                ValidateIssuer = appSettings.GetValue<bool>("ValidateIssuer"),
                ValidateAudience = appSettings.GetValue<bool>("ValidateAudience")
            };
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
            return validatedToken;
        }

        private byte[] GetSecretKey()
        {
            string keyString = _configuration["AppSettings:SecretKey"];
            return Encoding.ASCII.GetBytes(keyString);
        }
    }
}

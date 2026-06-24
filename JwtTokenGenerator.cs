using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using VisitorWebAPI.Core.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;



namespace VisitorWebAPI.Utilities.Security
{
    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(
            IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(
            UserEntity user)
        {
            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    user.Username),

                new Claim(
                    ClaimTypes.Role,
                    user.Role),

                new Claim(
                    "FullName",
                    user.FullName)
            };

            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        _configuration["JwtSettings:SecretKey"]));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var expiryMinutes =
                Convert.ToDouble(
                    _configuration["JwtSettings:ExpiryMinutes"]);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _configuration["JwtSettings:Issuer"],

                    audience:
                        _configuration["JwtSettings:Audience"],

                    claims: claims,

                    expires:
                        DateTime.Now.AddMinutes(expiryMinutes),

                    signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}

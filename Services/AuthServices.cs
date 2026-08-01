using TaskManagementAPI.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TaskManagementAPI.Services
{
    public class AuthServices
    {
        private readonly PasswordHasher<UserEntity> passwordHasher = new PasswordHasher<UserEntity>();
        private readonly IConfiguration _configuration;

        public AuthServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string HashPassword(UserEntity user, string password)
        {
            return passwordHasher.HashPassword(user, password);
        }

        public PasswordVerificationResult VerityPassword(UserEntity user, string hashedPassword, string providedPassword)
        {
            return passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }

        public string GenerateToken(UserEntity user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            string? keyValue = _configuration["Jwt:Key"];

            if (keyValue == null)
            {
                throw new Exception("Key can not be null");
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(keyValue));

            var credentials = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(10),
                signingCredentials: credentials
                );

            string jwt = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return jwt;
        }
    }
}

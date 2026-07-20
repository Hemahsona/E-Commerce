using E_Commerce.Application.Contract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static E_Commerce.Infrastructure.Services.TokenService;

namespace E_Commerce.Infrastructure.Services
{
    internal class TokenService(IOptions<JwtSettings> jwtOptions) : ITokenService
    {
        private readonly JwtSettings _jwtOptions = jwtOptions.Value;
        public string CreateToken(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            //Clamis
            var claims = new List<Claim>
            {
                 new (ClaimTypes.NameIdentifier, userId),
                 new (ClaimTypes.Email, email),
                 new (ClaimTypes.Name, userName)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            //signingCreate [scret key, security algorithm] 
            var secretKey = _jwtOptions.SecretKey;
            if(string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT Secret Key is not configured.");
            if(secretKey.Length < 32)
                throw new InvalidOperationException("JWT Secret Key must be at least 32 characters long.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationInMinutes),
                signingCredentials: credentials
                );
           return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public int ExpirationInMinutes { get; set; }
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
    }
}

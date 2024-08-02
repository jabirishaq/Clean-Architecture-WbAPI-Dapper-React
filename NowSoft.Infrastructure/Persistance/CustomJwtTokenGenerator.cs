using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using NowSoft.Domain.JwtToken;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NowSoft.Infrastructure.Persistance
{
    public class CustomJwtTokenGenerator : ICustomJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public CustomJwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
        {
            _jwtSettings = jwtOptions.Value;
        }

        public string GenerateToken(User user)
        {
            // Create signing credentials
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            // Define claims
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    
               new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()), // Use user.Id used here to get the id in balance endpoint

               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

            // Create the token
            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
                signingCredentials: signingCredentials);

            // Return the token
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NowSoft.Application.Interfaces;
using NowSoft.Domain.Entities;
using NowSoft.Domain.JwtToken;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NowSoft.Infrastructure.Persistance
{
    // CustomJwtTokenGenerator class implements the ICustomJwtTokenGenerator interface
    // and is responsible for generating JWT tokens for authenticated users.
    public class CustomJwtTokenGenerator : ICustomJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        // Constructor that injects JwtSettings through IOptions pattern
        public CustomJwtTokenGenerator(IOptions<JwtSettings> jwtOptions)
        {
            // Retrieve JWT configuration settings from the injected options
            _jwtSettings = jwtOptions.Value;
        }

        // Method to generate a JWT token for a given user
        public string GenerateToken(User user)
        {
            // Create signing credentials using the secret key from appsettings.json and HMAC SHA256 algorithm
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            // Define claims to include in the token, representing user information
            var claims = new[]
            {
                // Sub(Subject) claim to identify the user (username used as identifier)
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),

                // Sid(Session ID) claim to include the user's unique identifier (user ID)
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),

                // Jti(JWT ID) claim to provide a unique identifier for the token
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            //Create the security token using the JwtSecurityToken class that is defined in appsettings.json
            var securityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer, // The issuer of the token (server generating the token)
                audience: _jwtSettings.Audience, // The intended audience for the token (clients)
                claims: claims, // Claims included in the token to identify and authorize the user
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes), // Token expiration time
                signingCredentials: signingCredentials); // Signing credentials to validate the token's integrity

            //Serialize the token to a string and return it
            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CGP.Application.Utils
{
    public class TokenGenerators
    {
        private readonly IConfiguration _configuration;

        public TokenGenerators(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string accessToken, string refreshToken) GenerateTokens(IEnumerable<Claim>? claims = null)
        {
            var accessToken = GenerateToken(
                _configuration["AccessTokenSecret"],
                _configuration["Issuer"],
                _configuration["Audience"],
                double.Parse(_configuration["AccessTokenExpirationMinutes"]),
                claims
            );

            var refreshToken = GenerateToken(
                _configuration["RefreshTokenSecret"],
                _configuration["Issuer"],
                _configuration["Audience"],
                double.Parse(_configuration["refreshTokenExpirationMinutes"]),
                claims
            );

            return (accessToken, refreshToken);
        }

        private string GenerateToken(string secretKey, string issuer, string audience, double expirationMinutes, IEnumerable<Claim>? claims = null)
        {
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new(
                issuer,
                audience,
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(7).AddMinutes(expirationMinutes),
                credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateRefreshToken(string refreshToken)
        {
            TokenValidationParameters validationParameters = new()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["RefreshTokenSecret"])),
                ValidIssuer = _configuration["Issuer"],
                ValidAudience = _configuration["Audience"],
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero,
            };

            JwtSecurityTokenHandler tokenHandler = new();
            try
            {
                tokenHandler.ValidateToken(refreshToken, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

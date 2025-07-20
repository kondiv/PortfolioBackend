using Data.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Services.Services
{
    public sealed class JwtTokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _settings;

        public JwtTokenService(UserManager<ApplicationUser> userManager, JwtSettings settings)
        {
            _userManager = userManager;
            _settings = settings;
        }

        public async Task<Tokens> GenerateTokensAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            return new Tokens(
                GenerateJwtAccessToken(claims),
                GenerateRefreshToken(user.Id));
        }

        private string GenerateJwtAccessToken(List<Claim> claims)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_settings.LifetimeMinutes));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static RefreshToken GenerateRefreshToken(string userId)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            var tokenValue = Convert.ToBase64String(randomNumber);

            return new RefreshToken()
            {
                Token = tokenValue,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                UserId = userId,
                Revoked = null
            };
        }
    }
}

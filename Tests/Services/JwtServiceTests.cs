using FluentAssertions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Services.Services;
using Xunit;
using Data.Configuration;
using System.Security.Claims;
using Moq;

namespace Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtSettings _settings;
        private readonly JwtService _jwtService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public JwtServiceTests()
        {
            _settings = new JwtSettings()
            {
                Audience = "test-audience",
                Issuer = "test-issuer",
                SecretKey = "test-key-qwerty",
                LifetimeMinutes = 10
            };

            _jwtService = new JwtService(_settings);
        }

        [Fact]
        public async Task GenerateAccessTokenAsync_ShouldReturnValidJwt()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Name, "test-user")
            };

            // Act
            string token = _jwtService.GenerateAccessToken(claims);

            // Assert         
            token.Should().NotBeNullOrEmpty();

            var jwtToken = _jwtSecurityTokenHandler.ReadToken(token);

            jwtToken.Issuer.Should().Equals(_settings.Issuer);
        }

        [Fact]
        public void GenerateRefreshToken_ReturnsValidFormat()
        {
            // Act
            var token = _jwtService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(token);
            var parts = token.Split('.');
            Assert.Equal(2, parts.Length);
            Assert.True(long.TryParse(parts[0], out _)); // Проверяем timestamp
            Assert.Matches("^[A-Za-z0-9_-]+$", parts[1]); // Проверяем URL-safe Base64
            Assert.InRange(parts[1].Length, 40, 44); // Длина Base64 для 32 байт
        }
    }
}

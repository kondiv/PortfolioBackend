using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using Services.Services;
using Xunit;
using Data.Configuration;
using System.Security.Claims;
using Services.Interfaces;

namespace Tests.Services
{
    public class JwtServiceTests
    {
        private readonly JwtSettings _settings;
        private readonly ITokenService _jwtService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public JwtServiceTests()
        {
            _settings = new JwtSettings()
            {
                Audience = "test-audience",
                Issuer = "test-issuer",
                SecretKey = "32-Characters-Long-Key-1234567890!!",
                LifetimeMinutes = 10
            };

            _jwtService = new JwtTokenService(_settings);
        }

        [Fact]
        public void GenerateJwtAccessToken_ShouldReturnValidJwt()
        {
            // Arrange
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Name, "test-user")
            };

            // Act
            string token = _jwtService.GenerateJwtAccessToken(claims);

            // Assert         
            token.Should().NotBeNullOrEmpty();

            var jwtToken = _jwtSecurityTokenHandler.ReadToken(token);

            jwtToken.Issuer.Should().Be(_settings.Issuer);
        }

        [Fact]
        public void GenerateRefreshToken_ReturnsValidFormat()
        {
            // Act
            var token = _jwtService.GenerateRefreshToken();

            // Assert
            Assert.NotNull(token);
            Assert.InRange(token.Length, 40, 44); // Длина Base64 для 32 байт
        }
    }
}

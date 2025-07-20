using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using Services.Services;
using Xunit;
using Data.Configuration;
using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.Interfaces;

namespace Tests.Services
{
    public class JwtServiceTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly ITokenService _jwtService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

        public JwtServiceTests()
        {
            var mockUserStore = new Mock<IUserStore<ApplicationUser>>();

            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                mockUserStore.Object,
                null, 
                null, 
                null, // IEnumerable<IUserValidator<ApplicationUser>>
                null, // IEnumerable<IPasswordValidator<ApplicationUser>>
                null, // ILookupNormalizer
                null, // IdentityErrorDescriber
                null, // IServiceProvider
                null);
            
            var settings = new JwtSettings()
            {
                Audience = "test-audience",
                Issuer = "test-issuer",
                SecretKey = "32-Characters-Long-Key-1234567890!!",
                LifetimeMinutes = 10
            };

            _jwtService = new JwtTokenService(_mockUserManager.Object, settings);
        }

        [Fact]
        public async Task GenerateJwtAccessToken_ShouldReturnValidJwt()
        {
            // Arrange
            var user = new ApplicationUser()
            {
                Email = "email",
                UserName = "userName"
            };

            _mockUserManager
                .Setup(m => m.GetRolesAsync(user))
                .ReturnsAsync(new List<string>() { "Developer" });
            
            // Act
            var tokens = await _jwtService.GenerateTokensAsync(user);
            
            // Assert
            tokens.RefreshToken.Token.Should().NotBeNullOrEmpty();
            tokens.RefreshToken.UserId.Should().Be(user.Id);
            
            var accessToken = new JwtSecurityTokenHandler().ReadJwtToken(tokens.AccessToken);

            accessToken.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .Should().Contain("Developer");
            accessToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == user.Email);
            accessToken.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == user.UserName);
        }
    }
}

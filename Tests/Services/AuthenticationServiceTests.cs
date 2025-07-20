using Moq;
using Services.Interfaces;
using Data.Interfaces;
using Microsoft.Extensions.Logging;
using Services.Factories.RegistrationStrategyFactory;
using Services.Services;
using Xunit;

namespace Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IRegistrationStrategyFactory> _strategyFactoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly Mock<IRefreshTokenRepository> _refreshTokenRepoMock;
        private readonly Mock<ILogger<AuthenticationService>> _loggerMock;
        private readonly AuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            _strategyFactoryMock = new Mock<IRegistrationStrategyFactory>();
            _tokenServiceMock = new Mock<ITokenService>();
            _refreshTokenRepoMock = new Mock<IRefreshTokenRepository>();
            _loggerMock = new Mock<ILogger<AuthenticationService>>();
            
            _authService = new AuthenticationService(
                _strategyFactoryMock.Object,
                _tokenServiceMock.Object,
                _refreshTokenRepoMock.Object,
                _loggerMock.Object);
        }
        
        // TODO Write tests!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        [Fact]
        public async Task LoginAsync_When_UserExists_PasswordCorrect_ShouldReturnTokens()
        {
            // Arrange
            
        }
    }
}
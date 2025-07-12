using Data.Interfaces;
using Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Xunit;

namespace Tests.Repositories;

public class RefreshTokenRepositoryTests
{
    [Fact]
    public async Task AddTokenAsync_WhenTokenIsUnique_ShouldCreateRefreshToken()
    {
        // Arrange
        await using var dbContext = GetDbContext();
        IRefreshTokenRepository refreshTokenRepository = new RefreshTokenRepository(dbContext);
        
        var userId = Guid.NewGuid().ToString();
        
        var refreshToken = new RefreshToken()
        {
            CreatedAt = DateTime.Now,
            CreatedByIp = "127.0.0.1",
            ExpiresAt = DateTime.Now.AddDays(30),
            RefreshTokenId = Guid.NewGuid(),
            ReplacedByToken = null,
            Revoked = null,
            Token = "random-token",
            UserId = userId
        };
            
        // Act
        await refreshTokenRepository.AddRefreshTokenAsync(refreshToken);

        var result = await refreshTokenRepository.GetRefreshTokenAsync(userId);
        // Arrange
        result.Should().NotBeNull();
        
        result.Token.Should().Be("random-token");
    }
    
    private PortfolioContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PortfolioContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PortfolioContext(options);
    }
}
using Domain.Entities;

namespace Data.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<RefreshToken> GetRefreshTokenAsync(string userId, CancellationToken cancellationToken = default);
}
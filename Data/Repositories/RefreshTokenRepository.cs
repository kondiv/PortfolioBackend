using Data.Exceptions;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Data.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly PortfolioContext _context;

    public RefreshTokenRepository(PortfolioContext context)
    {
        _context = context;
    }
    
    public async Task AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(string userId, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == userId, cancellationToken);
        return token ?? throw new NotFoundException();
    }
}
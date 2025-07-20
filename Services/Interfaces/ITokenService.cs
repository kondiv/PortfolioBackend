using System.Security.Claims;
using Domain.Dto;
using Domain.Entities;

namespace Services.Interfaces
{
    public interface ITokenService
    {
        Task<Tokens> GenerateTokensAsync(ApplicationUser user);
    }
}

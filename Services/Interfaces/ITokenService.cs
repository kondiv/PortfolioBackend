using System.Security.Claims;

namespace Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtAccessToken(List<Claim> claims);
        string GenerateRefreshToken();
    }
}

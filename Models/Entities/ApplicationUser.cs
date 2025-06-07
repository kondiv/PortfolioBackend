using Microsoft.AspNetCore.Identity;

namespace Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}

using System.ComponentModel.DataAnnotations;

namespace Data.Configuration
{
    public sealed class JwtSettings
    {
        [Required]
        [MinLength(64)]
        public string SecretKey { get; init; } = string.Empty;
        [Required]
        public string Audience { get; init; } = string.Empty;
        [Required]
        public string Issuer { get; init; } = string.Empty;
        [Required]
        [Range(0, 1440)]
        public int LifetimeMinutes { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Data.Configuration
{
    public sealed class JwtSettings
    {
        [Required]
        [MinLength(64)]
        public string SecretKey { get; set; } = string.Empty;
        [Required]
        public string Audience { get; set; } = string.Empty;
        [Required]
        public string Issuer { get; set; } = string.Empty;
        [Required]
        [Range(0, 1440)]
        public int LifetimeMinutes { get; set; }
    }
}
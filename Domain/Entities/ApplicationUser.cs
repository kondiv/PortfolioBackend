using System.Text.Json.Serialization;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string? AvatarUrl { get; set; }

        public string? Bio { get; set; }
        public int? ExperienceYears { get; set; }
        public DeveloperLevel? DeveloperLevel { get; set; }
        public ICollection<UserSkill> UserSkills { get; set; } = [];

        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    }
}

using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Dto
{
    public class DeveloperRegistrationRequest : RegistrationRequest
    {
        public override string Role => "Developer";

        [Required]
        [MinLength(1)]
        public List<SkillDto> Skills { get; set; } = [];

        [Required]
        [Range(0, 4)]
        public DeveloperLevel DeveloperLevel { get; set; }

        [Required]
        [MaxLength(500)]
        public string Bio { get; set; } = string.Empty;
    }
}

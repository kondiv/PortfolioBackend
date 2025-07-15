using Domain.Enums;

namespace Domain.Dto.Authentication;

public record DeveloperRegistrationDto(
    string Email,
    string Password,
    string FullName,
    List<SkillDto> Skills,
    DeveloperLevel DeveloperLevel,
    int ExperienceYears,
    string Bio,
    string AvatarUrl) : IAuthenticationDto;
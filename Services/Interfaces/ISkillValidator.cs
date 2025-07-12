using Domain.Dto;

namespace Services.Interfaces;

public interface ISkillValidator
{
    Task<bool> ValidateSkillsAsync(List<SkillDto> skills, CancellationToken cancellationToken = default);
}
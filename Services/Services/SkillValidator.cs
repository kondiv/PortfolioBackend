using Data.Interfaces;
using Domain.Dto;
using Services.Interfaces;

namespace Services.Services;

public sealed class SkillValidator : ISkillValidator
{
    private readonly ISkillRepository _skillRepository;

    public SkillValidator(ISkillRepository skillRepository)
    {
        _skillRepository = skillRepository;
    }
    
    public async Task<bool> ValidateSkillsAsync(List<SkillDto> skills, CancellationToken cancellationToken)
    {
        var skillIds = skills.Select(skill => skill.SkillId).ToList();
        
        var skillsById = await _skillRepository.GetSkillsByIdAsync(skillIds, cancellationToken);
        
        return skills.Count == skillsById.Count();
    }
}
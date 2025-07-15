using Domain.Entities;

namespace Data.Interfaces;

public interface IUserSkillRepository
{
    Task AddRangeAsync(IEnumerable<UserSkill> userSkills, CancellationToken cancellationToken = default);
}
using Domain.Entities;

namespace Data.Interfaces;

public interface IUserSkillRepository
{
    Task AddAsync(UserSkill userSkill, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<UserSkill> userSkills, CancellationToken cancellationToken = default);
}
using Domain.Entities;

namespace Data.Interfaces;

public interface ISkillRepository
{
    Task<Skill> GetSkillAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Skill>> GetSkillsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Skill>> GetSkillsByIdAsync(List<int> ids, CancellationToken cancellationToken = default);
}
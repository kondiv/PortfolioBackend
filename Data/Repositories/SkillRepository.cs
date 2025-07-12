using Data.Exceptions;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Data.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly PortfolioContext _dbContext;

    public SkillRepository(PortfolioContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Skill> GetSkillAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Skills.FindAsync([id], cancellationToken) ?? throw new NotFoundException();
    }

    public async Task<IEnumerable<Skill>> GetSkillsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Skills.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Skill>> GetSkillsByIdAsync(List<int> ids, CancellationToken cancellationToken = default)
    {
        if (ids.Count == 0)
        {
            return [];
        }
            
        return await _dbContext.Skills.Where(skill => ids.Contains(skill.SkillId)).ToListAsync(cancellationToken);
    }
}
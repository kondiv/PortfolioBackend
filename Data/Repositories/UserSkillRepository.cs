using Data.Interfaces;
using Domain.Entities;

namespace Data.Repositories
{
    public class UserSkillRepository : IUserSkillRepository
    {
        private readonly PortfolioContext _context;

        public UserSkillRepository(PortfolioContext context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<UserSkill> userSkills, CancellationToken cancellationToken = default)
        {
            await _context.UserSkills.AddRangeAsync(userSkills, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}

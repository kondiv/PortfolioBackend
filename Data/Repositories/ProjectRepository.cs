using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly PortfolioContext _context;

        public ProjectRepository(PortfolioContext dbContext)
        {
            _context = dbContext;
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(project, cancellationToken: cancellationToken);
            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Projects.ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var project = await _context.Projects.FindAsync([id], cancellationToken: cancellationToken);

            if (project == null)
            { 
                return; 
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        }
    }
}
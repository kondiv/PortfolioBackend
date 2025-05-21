using Models.Entities;
using Data.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken = default)
        {
            await _projectRepository.AddAsync(project, cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _projectRepository.RemoveAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _projectRepository.GetAllAsync(cancellationToken);
        }
    }
}

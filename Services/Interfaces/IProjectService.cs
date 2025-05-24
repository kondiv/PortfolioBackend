using Models.Entities;

namespace Services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Project project, CancellationToken cancellationToken = default);
        Task RemoveAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

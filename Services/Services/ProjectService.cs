using Domain.Entities;
using Data.Interfaces;
using Services.Interfaces;
using Services.Validators;
using Domain.Exceptions;

namespace Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ProjectValidator _projectValidator;

        public ProjectService(
            IProjectRepository projectRepository,
            ProjectValidator projectValidator)
        {
            _projectRepository = projectRepository;
            _projectValidator = projectValidator;
        }

        public async Task AddAsync(Project project, CancellationToken cancellationToken)
        {
            var validationResult = _projectValidator.Validate(project);

            if (!validationResult.IsValid)
            {
                throw new InvalidModelException();
            }

            cancellationToken.ThrowIfCancellationRequested();

            await _projectRepository.AddAsync(project, cancellationToken);
        }

        public async Task RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            await _projectRepository.RemoveAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _projectRepository.GetAllAsync(cancellationToken);
        }
    }
}

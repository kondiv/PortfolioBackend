using Data.Exceptions;
using Data.Interfaces;
using FluentAssertions;
using Domain.Entities;
using Domain.Exceptions;
using Moq;
using Services.Interfaces;
using Services.Services;
using Services.Validators;
using Xunit;

namespace Tests.Services
{
    public class ProjectServiceTests
    {
        [Fact]
        public async Task GetAllAsync_WhenProjectsExist_ReturnsFilledList()
        {
            // Arrange
            var mockProjectRepository = new Mock<IProjectRepository>();
            var projectValidator = new ProjectValidator();

            var projectsToReturnByRepo = new List<Project>()
            {
                new Project {ProjectId = Guid.NewGuid(), Title="Title1", Description="Description1", GithubReference="Ref1"},
                new Project {ProjectId = Guid.NewGuid(), Title="Title2", Description="Description2", GithubReference="Ref2"},
                new Project {ProjectId = Guid.NewGuid(), Title="Title3", Description="Description3", GithubReference="Ref3"}
            };

            mockProjectRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectsToReturnByRepo);

            IProjectService projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            // Act
            var projects = await projectService.GetAllAsync();

            // Assert
            projects.Should().BeEquivalentTo(projectsToReturnByRepo);

            mockProjectRepository.Verify(
                repo => repo.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllAsync_WhenNoProjectsExist_ReturnsEmptyList()
        {
            // Arrange
            var mockProjectRepository = new Mock<IProjectRepository>();
            var projectValidator = new ProjectValidator();

            mockProjectRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            IProjectService projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            // Act
            var projects = await projectService.GetAllAsync();

            // Assert
            projects.Should().NotBeNull().And.BeEmpty();

            mockProjectRepository.Verify(
                repo => repo.GetAllAsync(It.IsAny<CancellationToken>()),
                Times.Once
            );
        }

        [Fact]
        public async Task GetAllAsync_WhenOperationIsCancelled_ThrowsException()
        {
            // Assert
            var mockProjectRepository = new Mock<IProjectRepository>();
            var projectValidator = new ProjectValidator();

            var cancellationToken = new CancellationToken(canceled: true);

            mockProjectRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            var service = new ProjectService(mockProjectRepository.Object, projectValidator);

            // Act
            Func<Task> act = () => service.GetAllAsync(cancellationToken);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }

        [Fact]
        public async Task AddAsync_WhenModelIsInvalid_ThrowsException()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var mockProjectRepository = new Mock<IProjectRepository>();

            IProjectService projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            var invalidProject = new Project();

            // Act
            Func<Task> act = () => projectService.AddAsync(invalidProject);

            // Assert
            await act.Should().ThrowAsync<InvalidModelException>();
        }

        [Fact]
        public async Task AddAsync_WhenOperationIsCanceled_ThrowsException()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var mockProjectRepository = new Mock<IProjectRepository>();
            var cancellationToken = new CancellationToken(canceled: true);

            var validProject = new Project()
            {
                ProjectId = Guid.NewGuid(),
                Title = "Title",
                Description = "Description",
                GithubReference = "https://github.com/kondiv/thoughtful"
            };

            var projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            // Act
            Func<Task> act = () => projectService.AddAsync(validProject, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
            mockProjectRepository
                .Verify(
                    repo => repo.AddAsync(validProject, cancellationToken),
                    Times.Never
                );
        }

        [Fact]
        public async Task AddAsync_WhenModelIsValid_CallsRepositoryMethod()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var mockProjectRepository = new Mock<IProjectRepository>();
            var cancellationToken = new CancellationToken(canceled: false);

            var validProject = new Project()
            {
                ProjectId = Guid.NewGuid(),
                Title = "Title",
                Description = "Description",
                GithubReference = "https://github.com/kondiv/thoughtful"
            };

            var projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            // Act
            Func<Task> act = () => projectService.AddAsync(validProject, cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();

            mockProjectRepository
                .Verify(
                    repo => repo.AddAsync(validProject, cancellationToken),
                    Times.Once
                );
        }

        [Fact]
        public async Task RemoveAsync_WhenOperationIsCanceled_ThrowsException()
        {
            // Arrange
            var projectValidator = new ProjectValidator();
            var mockProjectRepository = new Mock<IProjectRepository>();

            var projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            var cancellationToken = new CancellationToken(canceled: true);

            var projectId = Guid.NewGuid();

            // Act
            Func<Task> act = () => projectService.RemoveAsync(projectId, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();

            mockProjectRepository
                .Verify(
                    repo => repo.RemoveAsync(projectId, cancellationToken),
                    Times.Never
                );
        }

        [Fact]
        public async Task RemoveAsync_WhenSentIdIsNotFound_ThrowsException()
        {
            // Assert
            var randomId = Guid.NewGuid();

            var cancellationToken = new CancellationToken(canceled: false);

            var projectValidator = new ProjectValidator();

            var mockProjectRepository = new Mock<IProjectRepository>();
            mockProjectRepository
                .Setup(repo => repo.RemoveAsync(randomId, cancellationToken))
                .ThrowsAsync(new NotFoundException());

            var projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            // Act
            Func<Task> act = () => projectService.RemoveAsync(randomId, cancellationToken);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>();
            mockProjectRepository.Verify(
                    repo => repo.RemoveAsync(randomId, cancellationToken),
                    Times.Once
                );
        }

        [Fact]
        public async Task RemoveAsync_WhenSentIdIsFound_ChangesDb()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockProjectRepository = new Mock<IProjectRepository>();
            var projectValidator = new ProjectValidator();
            var cancellationToken = new CancellationToken(canceled: false);

            var projectService = new ProjectService(mockProjectRepository.Object, projectValidator);

            // Act
            Func<Task> act = () => projectService.RemoveAsync(id, cancellationToken);

            // Assert
            await act.Should().NotThrowAsync();
            mockProjectRepository.Verify(
                    repo => repo.RemoveAsync(id, cancellationToken),
                    Times.Once
                );
        }
    }
}

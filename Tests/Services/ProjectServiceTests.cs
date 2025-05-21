using Data.Interfaces;
using FluentAssertions;
using Models.Entities;
using Moq;
using Services.Services;
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

            var projectsToReturnByRepo = new List<Project>()
            {
                new Project {ProjectId = Guid.NewGuid(), Title="Title1", Description="Description1", GithubReference="Ref1"},
                new Project {ProjectId = Guid.NewGuid(), Title="Title2", Description="Description2", GithubReference="Ref2"},
                new Project {ProjectId = Guid.NewGuid(), Title="Title3", Description="Description3", GithubReference="Ref3"}
            };

            mockProjectRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectsToReturnByRepo);

            var projectService = new ProjectService(mockProjectRepository.Object);

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

            var projectsToReturnByRepo = new List<Project>();

            mockProjectRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(projectsToReturnByRepo);

            var projectService = new ProjectService(mockProjectRepository.Object);

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

            var cancellationToken = new CancellationToken(canceled: true);

            mockProjectRepository
                .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            var service = new ProjectService(mockProjectRepository.Object);

            // Act
            Func<Task> act = () => service.GetAllAsync(cancellationToken);

            // Assert
            await act.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}

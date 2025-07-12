using Api.Controllers;
using Data.Exceptions;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Exceptions;
using Moq;
using Services.Interfaces;
using Xunit;

namespace Tests.Controllers
{
    public class ProjectControllerTests
    {
        [Fact]
        public async Task GetAllAsync_WhenProjectsExist_ReturnsStatusCode200_With_Collection()
        {
            // Arrange
            var projects = new List<Project>()
            {
                new Project()
                {
                    Title = "Title",
                    Description = "Description",
                    GithubReference = "https://github.com/kondiv"
                },
                new Project()
                {
                    Title = "Title1",
                    Description = "Description1",
                    GithubReference = "https://github.com/kondiv1"
                },
                new Project()
                {
                    Title = "Title2",
                    Description = "Description2",
                    GithubReference = "https://github.com/kondiv2"
                }
            };

            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

            var projectsController = new ProjectsController(mockProjectService.Object);
            
            // Act
            var actionResult = await projectsController.GetAllAsync();

            // Assert
            var okResult = actionResult.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(200);

            var returnedProjects = okResult.Value as IEnumerable<Project>;
            returnedProjects.Should().NotBeNull();
            returnedProjects.Should().HaveCount(projects.Count());
            returnedProjects.Should().BeEquivalentTo(projects);
        }

        [Fact]
        public async Task GetAllAsync_WhenOperationIsCanceled_ThrowsException()
        {
            // Arrange
            var cancellationToken = new CancellationToken(canceled: true);
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.GetAllAsync(cancellationToken))
                .ThrowsAsync(new OperationCanceledException());

            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.GetAllAsync(cancellationToken);

            // Assert
            actionResult.Result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(499);
        }

        [Fact]
        public async Task GetAllAsync_WhenNoProjectsExist_ReturnStatusCode200OK_With_EmptyList()
        {
            // Arrange
            var projects = new List<Project>();
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(projects);

            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.GetAllAsync();

            // Arrange
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);

            var returnedProjects = result.Value as IEnumerable<Project>;
            returnedProjects.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public async Task AddAsync_WhenRightDataProvided_ReturnsNoContent()
        {
            // Arrange
            var mockProjectService = new Mock<IProjectService>();

            string title = "Title";
            string description = "Description";
            string githubReference = "https://github.com/kondiv";

            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.AddAsync(title, description, githubReference);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task AddAsync_WhenWrongDataProvided_ReturnsBadRequest()
        {
            // Arrange
            string invalidTitle = "t";
            string invalidDescription = "desc";
            string invalidGithubReference = "https://youtube.com";

            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidModelException());

            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.AddAsync(invalidTitle, invalidDescription, invalidGithubReference);

            // Assert
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task AddAsync_WhenOperationCancelled_ReturnsStatusCode499()
        {
            // Arrange
            string title = "Title";
            string description = "Descritpion";
            string githubReference = "https://github.com/kondiv";

            var cancellationToken = new CancellationToken(canceled: true);
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());
            
            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.AddAsync(title, description, githubReference, cancellationToken);

            // Assert
            actionResult.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(499);
        }

        [Fact]
        public async Task AddAsync_WhenNoSpecificResponseForException_Returns_BadRequestResult()
        {
            // Arrange
            string title = "Title";
            string description = "Descritpion";
            string githubReference = "https://github.com/kondiv";

            var cancellationToken = new CancellationToken(canceled: false);
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.AddAsync(title, description, githubReference, cancellationToken);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task RemoveAsync_WhenIdIsFound_Returns_NoContentResult()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockProjectService = new Mock<IProjectService>();
            
            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.RemoveAsync(id);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task RemoveAsync_WhenIdIsNotFound_Returns_NotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.RemoveAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new NotFoundException());
            
            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.RemoveAsync(id);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task RemoveAsync_WhenOperationCanceled_Returns_499StatusCodeResult()
        {
            // Arrange
            var cancellationToken = new CancellationToken(canceled: true);
            var id = Guid.NewGuid();
            var mockProjectService = new Mock<IProjectService>();
            mockProjectService
                .Setup(service => service.RemoveAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new OperationCanceledException());

            var projectsController = new ProjectsController(mockProjectService.Object);

            // Act
            var actionResult = await projectsController.RemoveAsync(id, cancellationToken);

            // Assert
            actionResult.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(499);
        }
    }
}

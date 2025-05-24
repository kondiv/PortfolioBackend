using Data.Exceptions;
using Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Xunit;

namespace Tests.Repositories;

public class ProjectRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_WhenNoProjectsExist_ReturnsEmptyList()
    {
        // Arrange
        using var dbContext = GetDbContext();
        var projectRepository = new ProjectRepository(dbContext);

        // Act
        var projects = await projectRepository.GetAllAsync();

        // Assert
        projects.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetAllAsync_WhenOperationIsCanceled_ThrowsException()
    {
        // Arrange
        using var dbContext = GetDbContext();
        var projectRepository = new ProjectRepository(dbContext);

        var cancellationToken = GetCanceledCancellationToken();

        // Act
        Func<Task> act = () => projectRepository.GetAllAsync(cancellationToken);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetAllAsync_WhenProjectsExist_ReturnsList()
    {
        // Arrange
        var firstProjectGuid = Guid.NewGuid();
        var firstProjectTitle = "Первый проект";

        using var dbContext = GetDbContext();
        dbContext.Projects.AddRange(
            new Project { ProjectId = firstProjectGuid, Title = firstProjectTitle, Description = "Описание", GithubReference = "Reference" },
            new Project { ProjectId = Guid.NewGuid(), Title = "Заголовок", Description = "Описание", GithubReference = "Reference" },
            new Project { ProjectId = Guid.NewGuid(), Title = "Заголовок", Description = "Описание", GithubReference = "Reference" }
            );
        dbContext.SaveChanges();

        var projectRepository = new ProjectRepository(dbContext);

        // Act
        var projects = await projectRepository.GetAllAsync();

        // Assert
        projects.Should().NotBeNullOrEmpty();
        projects.Should().HaveCount(3);
        projects.First().ProjectId.Should().Be(firstProjectGuid);
        projects.First().Title.Should().Be(firstProjectTitle);
    }

    [Fact]
    public async Task RemoveAsync_WhenSentIdMatchesProjectId_RemovesProjectWithSameId()
    {
        // Arrange
        var firstProjectId = Guid.NewGuid();

        var dbContext = GetDbContext();
        dbContext.Projects.AddRange(
            new Project { ProjectId = firstProjectId, Title = "Заголовок", Description = "Описание", GithubReference = "Reference" },
            new Project { ProjectId = Guid.NewGuid(), Title = "Заголовок", Description = "Описание", GithubReference = "Reference" },
            new Project { ProjectId = Guid.NewGuid(), Title = "Заголовок", Description = "Описание", GithubReference = "Reference" }
        );
        dbContext.SaveChanges();

        var projectRepository = new ProjectRepository(dbContext);

        // Act
        await projectRepository.RemoveAsync(firstProjectId);

        var projects = await projectRepository.GetAllAsync();

        // Assert
        projects.Should().HaveCount(2);

    }

    [Fact]
    public async Task RemoveAsync_WhenSentIdDoesNotMatchAnyProjectId_DoesNotChangeDb()
    {
        // Arrange
        var dbContext = GetDbContext();

        var projectRepository = new ProjectRepository(dbContext);

        // Act
        Func<Task> act = () => projectRepository.RemoveAsync(Guid.NewGuid());

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task RemoveAsync_WhenOperationIsCanceled_ThrowsException()
    {
        // Arrange
        var cancellationToken = GetCanceledCancellationToken();

        var dbContext = GetDbContext();
        var projectRepository = new ProjectRepository(dbContext);

        // Act
        Func<Task> act = () => projectRepository.RemoveAsync(Guid.NewGuid(), cancellationToken);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task AddAsync_WhenModelIsInvalid_ThrowsException()
    {
        // Arrange
        var dbContext = GetDbContext();
        var projectRepository = new ProjectRepository(dbContext);

        var invalidProjectModel = new Project();

        // Act
        Func<Task> act = () => projectRepository.AddAsync(invalidProjectModel);

        // Assert
        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task AddAsync_WhenModelIsValid_ChangesDb()
    {
        // Arrange
        var dbContext = GetDbContext();
        var projectRepository = new ProjectRepository(dbContext);

        var validProjectModel = new Project { Title = "Title", Description = "Description", GithubReference = "Reference" };

        // Act
        await projectRepository.AddAsync(validProjectModel);

        var result = await projectRepository.GetAllAsync();

        // Assert
        result.Should().HaveCount(1);
    }

    [Fact]
    public async Task AddAsync_WhenOperationIsCanceled_ThrowsException()
    {
        // Arrange
        var cancellationToken = GetCanceledCancellationToken();

        var dbContext = GetDbContext();
        var projectRepository = new ProjectRepository(dbContext);

        var validProjectModel = new Project { Title = "Title", Description = "Description", GithubReference = "Reference" };

        // Act
        Func<Task> act = () => projectRepository.AddAsync(validProjectModel, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    private PortfolioContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PortfolioContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PortfolioContext(options);
    }

    private CancellationToken GetCanceledCancellationToken()
    {
        var cts = new CancellationTokenSource();

        cts.Cancel();

        return cts.Token;
    }
}

using Data.Exceptions;
using Data.Interfaces;
using Data.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Xunit;

namespace Tests.Repositories;

public class SkillsRepositoryTests
{
    [Fact]
    public async Task GetSkillAsync_WhenSkillExists_ShouldReturnSkillEntity()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);

        await dbContext.Skills.AddAsync(new Skill() { Name = "C#", SkillId = 1});
        await dbContext.SaveChangesAsync();
        
        // Act
        var skill = await skillRepository.GetSkillAsync(1);
        
        // Assert
        skill.Name.Should().Be("C#");
    }

    [Fact]
    public async Task GetSkillAsync_WhenSkillDoesNotExist_ShouldThrowNotFoundException()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);
        
        // Act
        Func<Task> act = async () => await skillRepository.GetSkillAsync(1);
        
        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetSkillAsync_WhenOperationCanceled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);
        
        var cancellationToken = new CancellationToken(canceled: true);
        
        // Act
        Func<Task> act = async () => await skillRepository.GetSkillAsync(1, cancellationToken);
        
        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetSkillsAsync_WhenSkillsExist_ShouldReturn_CollectionOfSkills()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);

        var skillsToAdd = new List<Skill>()
        {
            new Skill() { SkillId = 1, Name = "C#" },
            new Skill() { SkillId = 2, Name = "Java" },
            new Skill() { SkillId = 3, Name = "C++" }
        };
        
        await dbContext.AddRangeAsync(skillsToAdd);
        await dbContext.SaveChangesAsync();
        
        // Act
        var skills = await skillRepository.GetSkillsAsync();
        
        // Assert
        var skillList = skills.ToList();
        skillList.Should().NotBeNull().And.NotBeEmpty();
        skillList.Should().HaveCount(3);
    }

    [Fact]
    public async Task GetSkillsAsync_WhenSkillsDoesNotExist_ShoulReturn_EmptyCollection()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);
        
        // Act
        var skills = await skillRepository.GetSkillsAsync();
        
        // Assert
        skills.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetSkillsAsync_WhenOperationCanceled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);
        
        var cancellationToken = new CancellationToken(canceled: true);
        
        // Act
        Func<Task> act = async () => await skillRepository.GetSkillsAsync(cancellationToken);
        
        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetSkillsByIdAsync_WhenSkillsExist_ShouldReturn_CollectionOfSkills_WithSameIds()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);

        var skills = new List<Skill>()
        {
            new Skill() { SkillId = 1, Name = "C#" },
            new Skill() { SkillId = 2, Name = "Java" },
            new Skill() { SkillId = 3, Name = "C++" }
        };

        var skillIds = new List<int>() { 1, 3, 5 };

        await dbContext.AddRangeAsync(skills);
        await dbContext.SaveChangesAsync();
        
        // Act
        var result = await skillRepository.GetSkillsByIdAsync(skillIds);
        
        // Assert
        var resultList = result.ToList();
        resultList.Should().NotBeNull().And.NotBeEmpty();
        resultList.Should().HaveCount(2);
        resultList.Should().Contain(skills.First());
        resultList.Should().Contain(skills.Last());
    }

    [Fact]
    public async Task GetSkillsByIdAsync_WhenSkillsDoNotExist_ShouldReturn_EmptyCollection()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);
        
        var skillIds = new List<int>() { 1, 3 };
        
        // Act
        var result = await skillRepository.GetSkillsByIdAsync(skillIds);
        
        // Assert
        result.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public async Task GetSkillsByIdAsync_WhenOperationCanceled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);
        
        var cancellationToken = new CancellationToken(canceled: true);
        
        var skillIds = new List<int>() { 1, 3 };
        
        // Act
        Func<Task> act = async () => await skillRepository.GetSkillsByIdAsync(skillIds, cancellationToken);
        
        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task GetSkillsByIdAsync_WhenEmptyIdListProvided_ShouldReturn_EmptyCollection()
    {
        // Arrange
        var dbContext = GetDbContext();
        ISkillRepository skillRepository = new SkillRepository(dbContext);

        var skillIds = new List<int>() { 1, 3 };
        
        // Act
        var skills = await skillRepository.GetSkillsByIdAsync(skillIds);
        
        // Assert
        skills.Should().NotBeNull().And.BeEmpty();
    }
    
    private PortfolioContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PortfolioContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PortfolioContext(options);
    }
}
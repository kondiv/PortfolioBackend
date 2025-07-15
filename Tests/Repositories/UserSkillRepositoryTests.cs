using Data.Interfaces;
using Data.Repositories;
using Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests.Repositories;

public class UserSkillRepositoryTests
{
    [Fact]
    public async Task AddAsync_WhenDataIsValid_ShouldAddUserSkill_To_Database()
    {
        // Arrange
        var context = GetDbContext();
        IUserSkillRepository userSkillRepository = new UserSkillRepository(context);

        var userSkillsToAdd = new List<UserSkill>()
        {
            new()
            {
                UserId = "1",
                SkillId = 1,
                Proficiency = 2
            },
            new()
            {
                UserId = "1",
                SkillId = 3,
                Proficiency = 4
            },
            new()
            {
                UserId = "1",
                SkillId = 2,
                Proficiency = 5
            }
        };
        
        // Act
        await userSkillRepository.AddRangeAsync(userSkillsToAdd);

        // Assert
        var userSkills = await context.UserSkills.ToListAsync();

        userSkills.Should().NotBeNull().And.NotBeEmpty();
        userSkills.Should().HaveCount(3);
    }
    
    private PortfolioContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PortfolioContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PortfolioContext(options);
    }
}
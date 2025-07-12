using Data.Interfaces;
using Domain.Entities;
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

        var userSkill = new UserSkill()
        {
            SkillId = 1,
            UserId = "1",
            Proficiency = 2
        };
        
        // Act
        await userSkillRepository.AddAsync(userSkill);
    }
    
    private PortfolioContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<PortfolioContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new PortfolioContext(options);
    }
}
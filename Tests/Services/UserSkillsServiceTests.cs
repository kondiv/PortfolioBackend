using Data.Interfaces;
using Domain.Dto;
using Domain.Entities;
using Domain.Values;
using Moq;
using Services.Interfaces;
using Services.Services;
using Xunit;

namespace Tests.Services;

public class UserSkillsServiceTests
{
    [Fact]
    public async Task AddRangeAsync_WhenDataIsValid_Should_AddUserSkill_To_Database()
    {
        // Arrange
        var mockUserSkillRepository = new Mock<IUserSkillRepository>();
        var mockSkillValidator = new Mock<ISkillValidator>();

        mockSkillValidator
            .Setup(v => v.ValidateSkillsAsync(It.IsAny<List<SkillDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        IUserSkillService userSkillService = new UserSkillService(
            mockSkillValidator.Object,
            mockUserSkillRepository.Object);

        var userId = Guid.NewGuid().ToString();
        var skills = new List<SkillDto>()
        {
            new SkillDto(1, new Proficiency(5)),
            new SkillDto(2, new Proficiency(3)),
            new SkillDto(3, new Proficiency(4)),
        };

        // Act
        await userSkillService.AddRangeAsync(userId, skills);

        // Assert
        mockSkillValidator.Verify(v =>
            v.ValidateSkillsAsync(It.Is<List<SkillDto>>(list => list.Equals(skills)), It.IsAny<CancellationToken>()),
            Times.Once);

        mockUserSkillRepository.Verify(r =>
            r.AddRangeAsync(It.IsAny<List<UserSkill>>(), It.IsAny<CancellationToken>()),
            Times.Once);

    }
}
using Data.Interfaces;
using FluentAssertions;
using Domain.Dto;
using Domain.Entities;
using Domain.Values;
using Moq;
using Services.Interfaces;
using Xunit;
using Services.Validators;

namespace Tests.Validators;

public class SkillValidatorTests
{
    [Fact]
    public async Task ValidateSkillAsync_WhenSkillNotExists_ShouldReturnFalse()
    {
        // Arrange
        var skillRepositoryMock = new Mock<ISkillRepository>();
        ISkillValidator skillValidator = new SkillValidator(skillRepositoryMock.Object);
        var skillDto = new SkillDto(1, new Proficiency(4));
        var skillDtoList = new List<SkillDto> { skillDto };

        skillRepositoryMock
            .Setup(r => r.GetSkillsByIdAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        
        // Act
        var exist = await skillValidator.ValidateSkillsAsync(skillDtoList);
        
        // Assert
        exist.Should().BeFalse();
        
        skillRepositoryMock.Verify(
            r => r.GetSkillsByIdAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ValidateSkillAsync_WhenSkillDoesNotExist_ShouldReturnFalse()
    {
        // Arrange
        var skillRepositoryMock = new Mock<ISkillRepository>();
        ISkillValidator skillValidator = new SkillValidator(skillRepositoryMock.Object);
        var skillDto = new SkillDto(1, new Proficiency(4));
        var skillDtoList = new List<SkillDto> { skillDto };

        skillRepositoryMock
            .Setup(r => r.GetSkillsByIdAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([new Skill() { Name = "C#", SkillId = skillDto.SkillId, UserSkills = null }]);
        
        // Act
        var exist = await skillValidator.ValidateSkillsAsync(skillDtoList);
        
        // Assert
        exist.Should().BeTrue();
        skillRepositoryMock.Verify(
            r => r.GetSkillsByIdAsync(It.IsAny<List<int>>(), It.IsAny<CancellationToken>()),
            Times.Once());
    }
    
    [Fact]
    public async Task ValidateSkillsAsync_WhenOperationCanceled_ShouldThrowOperationCanceledException()
    {
        // Arrange
        var skillRepositoryMock = new Mock<ISkillRepository>();
        ISkillValidator skillValidator = new SkillValidator(skillRepositoryMock.Object);
        var skillDto = new SkillDto(1, new Proficiency(4));
        var skillDtoList = new List<SkillDto> { skillDto };
        
        var cancellationToken = new CancellationToken(canceled: true);
        
        skillRepositoryMock
            .Setup(r => r.GetSkillsByIdAsync(It.IsAny<List<int>>(), cancellationToken))
            .ThrowsAsync(new OperationCanceledException());
        
        // Act
        var act = () => skillValidator.ValidateSkillsAsync(skillDtoList,  cancellationToken);
        
        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }
}
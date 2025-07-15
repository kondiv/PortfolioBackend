using FluentAssertions;
using Mapster;
using Domain.Dto;
using Domain.Entities;
using Domain.Enums;
using Services.Mapper;
using Xunit;
using Domain.Dto.Authentication;

namespace Tests.Mappers;

public class ApplicationUserMapperTests
{
    public ApplicationUserMapperTests()
    {
        ApplicationUserMapper.Configure();
    }
    
    [Fact]
    public void DeveloperRegistrationDto_To_ApplicationUser_ShouldMapCorrectly()
    {
        const string email = "test@email.ru";
        const string password = "password";
        const string fullName = "Тестов Тест Тестович";
        var skills = new List<SkillDto>();
        const DeveloperLevel developerLevel = DeveloperLevel.Junior;
        const int experienceYears = 5;
        const string bio = "тестовая биография для пользоователя-разработчика";
        const string avatarUrl = "pinterest.ru/avatar";
            
        // Arrange
        var dto = new DeveloperRegistrationDto(
            Email: email,
            Password: password,
            FullName: fullName,
            Skills: skills,
            DeveloperLevel: developerLevel,
            ExperienceYears: experienceYears,
            Bio: bio,
            AvatarUrl: avatarUrl
        );
        
        // Act
        var user = dto.Adapt<ApplicationUser>();
        
        // Assert
        user.Email.Should().Be(email);
        user.UserName.Should().Be(email);
        user.FullName.Should().Be(fullName);
        user.DeveloperLevel.Should().Be(developerLevel);
        user.ExperienceYears.Should().Be(experienceYears);
        user.Bio.Should().Be(bio);
        user.AvatarUrl.Should().Be(avatarUrl);
        user.UserSkills.Should().BeNullOrEmpty();
        user.PasswordHash.Should().BeNullOrEmpty();
    }

    [Fact]
    public void EmployerRegistrationDto_To_ApplicationUser_ShouldMapCorrectly()
    {
        // Arrange
        const string email = "test@email.ru";
        const string password = "password";
        const string fullName = "Тестов Тест Тестович";
        const string avatarUrl = "pinterest.ru/avatar";

        var dto = new EmployerRegistrationDto(fullName, email, password, avatarUrl);
        
        // Act
        var user = dto.Adapt<ApplicationUser>();
        
        // Assert
        user.UserName.Should().Be(email);
        user.Email.Should().Be(email);
        user.FullName.Should().Be(fullName);
        user.PasswordHash.Should().BeNullOrEmpty();
        user.AvatarUrl.Should().Be(avatarUrl);
        user.ExperienceYears.Should().BeNull();
        user.DeveloperLevel.Should().BeNull();
        user.Bio.Should().BeNull();
        user.UserSkills.Should().BeNullOrEmpty();
    }
}
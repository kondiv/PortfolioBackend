using Domain.Dto;
using Domain.Dto.Authentication;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Services.Interfaces;
using Services.Results;
using Services.Strategies.Registration;
using Xunit;

namespace Tests.Strategies;

public class RegistrationStrategiesTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<IUserSkillService> _mockUserSkillService;
    
    private readonly DeveloperRegistrationDto _developerRegistrationDto = new DeveloperRegistrationDto(
        "email@yandex.ru",
        "password",
        "Full Name",
        new List<SkillDto>(),
        DeveloperLevel.Junior,
        5,
        "Bio",
        "https://avattar.ru");

    public RegistrationStrategiesTests()
    {
        var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
        
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            mockUserStore.Object,
            null,  // IOptions<IdentityOptions>
            null,  // IPasswordHasher<ApplicationUser>
            null,  // IEnumerable<IUserValidator<ApplicationUser>>
            null,  // IEnumerable<IPasswordValidator<ApplicationUser>>
            null,  // ILookupNormalizer
            null,  // IdentityErrorDescriber
            null,  // IServiceProvider
            null); // ILogger<UserManager<ApplicationUser>>
        
        _mockUserSkillService = new Mock<IUserSkillService>();
    }
    
    [Fact]
    public async Task DeveloperRegistrationStrategy_Register_WhenDataIsValid_ShouldReturn_SuccessfulRegistrationResult()
    {
        // Arrange
        var testUser = new ApplicationUser { 
            Id = Guid.NewGuid().ToString(),
            Email = _developerRegistrationDto.Email,
            UserName = _developerRegistrationDto.Email
        };

        _mockUserManager
            .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<ApplicationUser, string>((user, _) => 
            {
                // Заполняем поля пользователя, которые должны быть установлены
                user.Id = testUser.Id;
                user.Email = testUser.Email;
                user.UserName = testUser.UserName;
            });

        _mockUserManager
            .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _mockUserSkillService
            .Setup(s => s.AddRangeAsync(It.IsAny<string>(), It.IsAny<List<SkillDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success);
        
        var strategy = new DeveloperRegistrationStrategy(_developerRegistrationDto, _mockUserManager.Object, _mockUserSkillService.Object);
        
        // Act
        var result = await strategy.Register();
        
        // Assert
        result.Succeeded.Should().BeTrue();
        result.User.Should().NotBeNull();
        result.Errors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task
        DeveloperRegistrationStrategy_Register_WhenUserIsNotCreated_ShouldReturn_FailedRegistrationResult()
    {
        // Arrange
        _mockUserManager
            .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());
        
        var strategy = new DeveloperRegistrationStrategy(_developerRegistrationDto, _mockUserManager.Object, _mockUserSkillService.Object);
        
        // Act
        var result = await strategy.Register();
        
        // Assert
        result.Succeeded.Should().BeFalse();
        result.User.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }

    [Fact]
    public async Task
        DeveloperRegistrationStrategy_Register_When_FailedToAddToRole_ShouldReturn_FailedRegistrationResult()
    {
        // Arrange
        var testUser = new ApplicationUser { 
            Id = Guid.NewGuid().ToString(),
            Email = _developerRegistrationDto.Email,
            UserName = _developerRegistrationDto.Email
        };

        _mockUserManager
            .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<ApplicationUser, string>((user, _) => 
            {
                // Заполняем поля пользователя, которые должны быть установлены
                user.Id = testUser.Id;
                user.Email = testUser.Email;
                user.UserName = testUser.UserName;
            });
        
        _mockUserManager
            .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed());
        
        var strategy = new DeveloperRegistrationStrategy(_developerRegistrationDto, _mockUserManager.Object, _mockUserSkillService.Object);
        
        // Act
        var result = await strategy.Register();
        
        // Assert
        result.Succeeded.Should().BeFalse();
        result.User.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }

    [Fact]
    public async Task
        DeveloperRegistrationStrategy_Register_When_SkillsAreNotAssigned_ShouldReturn_FailedRegistrationResult()
    {
        // Arrange
        var testUser = new ApplicationUser { 
            Id = Guid.NewGuid().ToString(),
            Email = _developerRegistrationDto.Email,
            UserName = _developerRegistrationDto.Email
        };

        _mockUserManager
            .Setup(m => m.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<ApplicationUser, string>((user, _) => 
            {
                user.Id = testUser.Id;
                user.Email = testUser.Email;
                user.UserName = testUser.UserName;
            });

        _mockUserManager
            .Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        
        _mockUserSkillService
            .Setup(s => s.AddRangeAsync(It.IsAny<string>(), It.IsAny<List<SkillDto>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failed());
        
        var strategy = new DeveloperRegistrationStrategy(_developerRegistrationDto, _mockUserManager.Object, _mockUserSkillService.Object);
        
        // Act
        var result = await strategy.Register();
        
        // Assert
        result.Succeeded.Should().BeFalse();
        result.User.Should().BeNull();
        result.Errors.Should().NotBeNull();
    }
}
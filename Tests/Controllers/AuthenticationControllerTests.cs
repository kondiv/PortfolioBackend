// using Api.Controllers;
// using FluentAssertions;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;
// using Domain.Dto;
// using Domain.Entities;
// using Domain.Enums;
// using Moq;
// using Xunit;
//
// namespace Tests.Controllers
// {
//     public class AuthenticationControllerTests
//     {
//         private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
//         private readonly Mock<RoleManager<ApplicationRole>> _mockRoleManager;
//         private readonly AuthenticationController _authController;
//
//         public AuthenticationControllerTests()
//         {
//             var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
//             _mockUserManager = new Mock<UserManager<ApplicationUser>>(mockUserStore.Object,
//                 null, null, null, null, null, null, null, null);
//
//             var mockRoleStore = new Mock<IRoleStore<ApplicationRole>>();
//             _mockRoleManager = new Mock<RoleManager<ApplicationRole>>(mockRoleStore.Object,
//                 null, null, null, null);
//
//             _authController = new AuthenticationController(_mockUserManager.Object, _mockRoleManager.Object);
//         }
//             
//         [Fact]
//         public async Task RegisterDeveloperAsync_WhenDataIsValid_ShouldReturn_StatusCode_204_Created()
//         {
//             // Arrange
//             var registrationRequest = new DeveloperRegistrationDto(
//                 "test-email@yandex.ru",
//                 "test-password",
//                 "Кондрашин Илья Валерьевич",
//                 [new SkillDto(1, 4), new SkillDto(2, 5)],
//                 DeveloperLevel.Junior,
//                 5,
//                 "test short bio for developer registration request",
//                 "https::avatarki/avatarka/1234123142");
//
//             _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
//                 .ReturnsAsync(IdentityResult.Success);
//
//             _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
//                 .ReturnsAsync(IdentityResult.Success);
//
//             // Act
//             var result = await _authController.RegisterDeveloperAsync(registrationRequest);
//
//             // Assert
//             result.Should().BeOfType<CreatedAtActionResult>();
//
//             _mockUserManager.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registrationRequest.Password), Times.Once);
//             _mockUserManager.Verify(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), "Developer"), Times.Once);
//         }
//     }
// }

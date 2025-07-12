// using Domain.Dto;
// using Moq;
// using Xunit;
// using Services.Interfaces;
// using System.Security.Claims;
// using FluentAssertions;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Identity;
// using Domain.Entities;
// using IAuthenticationService = Services.Interfaces.IAuthenticationService;
//
// namespace Tests.Services
// {
//     public class AuthenticationServiceTests
//     {
//         private readonly IAuthenticationService _authenticationService;
//
//         private readonly Mock<ITokenService> _tokenServiceMock;
//         private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
//
//         public AuthenticationServiceTests()
//         {
//             _tokenServiceMock = new Mock<ITokenService>();
//
//             _tokenServiceMock.Setup(x => x.GenerateJwtAccessToken(It.IsAny<List<Claim>>()))
//                 .Returns("generated-access-token");
//
//             _tokenServiceMock.Setup(x => x.GenerateRefreshToken())
//                 .Returns("generated-refresh-token");
//
//             var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
//             _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object,
//                 null, null, null, null, null, null, null, null);
//
//             _authenticationService = new AuthenticationService(_userManagerMock.Object, _tokenServiceMock.Object);
//         }
//
//         [Fact]
//         public async Task RegisterDeveloperAsync()
//         {
//             // Arrange
//             var developerRegistrationDto = new DeveloperRegistrationDto(
//                 Email: "test.developer@yandex.ru",
//                 Password: "TestPassword123",
//                 FullName: "Кондрашин Илья Валерьевич",
//                 Skills: new List<SkillDto>() { new(1, 5), new(2, 3) },
//                 DeveloperLevel: Domain.Enums.DeveloperLevel.Junior,
//                 ExperienceYears: 1,
//                 Bio: "test short developer's bio",
//                 AvatarUrl: "server/avatar.png");
//             
//             _userManagerMock
//                 .Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>()))
//                 .ReturnsAsync(IdentityResult.Success);
//             
//             _userManagerMock
//                 .Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
//                 .ReturnsAsync(IdentityResult.Success);
//             
//             // Act
//             var result = await _authenticationService.RegisterDeveloperAsync(developerRegistrationDto);
//             
//             // Assert
//             result.Should().Be(IdentityResult.Success);
//
//             _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<ApplicationUser>(), developerRegistrationDto.Password),
//                 Times.Once);
//             _userManagerMock.Verify(x => x.AddToRoleAsync(
//                     It.Is<ApplicationUser>(u => u.Email == developerRegistrationDto.Email),
//                     "Developer"),
//                 Times.Once);
//             
//             _tokenServiceMock.Verify(x => x.GenerateJwtAccessToken(It.IsAny<List<Claim>>()), Times.Once);
//             _tokenServiceMock.Verify(x => x.GenerateRefreshToken(), Times.Once);
//         }
//     }
// }
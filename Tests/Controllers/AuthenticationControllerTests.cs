using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using Xunit;

namespace Tests.Controllers
{
    //public class AuthenticationControllerTests
    //{
    //    private readonly AuthenticationController _authController;

    //    public AuthenticationControllerTests()
    //    {
    //        IAuthenticationService authService = new AuthenticationService();
    //        _authController = new AuthenticationController(authService);
    //    }

    //    [Fact]
    //    public async Task RegisterDeveloperAsync_WhenDataIsValid_ShouldReturn_StatusCode_204_Created()
    //    {
    //        // Arrange

    //        RegistrationRequest registrationRequest = new DeveloperRegistrationRequest()
    //        {
    //            FullName = "My Name Test",
    //            EmailAddress = "test@mail.ru",
    //            Password = "test_password",
    //            Bio = "test bio for developer user",
    //            DeveloperLevel = Models.Enums.DeveloperLevel.Senior,
    //            Skills = 
    //            [
    //                new SkillDto(1, 4),
    //                new SkillDto(2, 5)
    //            ]
    //        };

    //        // Act
    //        ActionResult result = await _authController.RegisterDeveloperAsync(registrationRequest);

    //        // Assert
    //        result.Should().BeOfType<CreatedAtActionResult>();
    //    }
    //}
}

using Domain.Dto;
using Domain.Entities;
using FluentAssertions;
using Mapster;
using Services.Mapper;

namespace Tests.Mappers;

public class UserDtoMapperTests
{
    public UserDtoMapperTests()
    {
        UserDtoMapper.Configure();
    }

    public void ApplicationUser_To_UserDto_ShouldMapCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();
        const string email = "email@yandex.ru";
        const string username = email;

        var user = new ApplicationUser()
        {
            Id = id,
            Email = email,
            UserName = username
        };
        
        // Act
        var userDto = user.Adapt<UserDto>();
        
        // Assert
        userDto.Id.Should().Be(id);
        userDto.Email.Should().Be(email);
        userDto.UserName.Should().Be(username);
    }
}
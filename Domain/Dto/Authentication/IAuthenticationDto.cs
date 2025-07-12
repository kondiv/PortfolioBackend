namespace Domain.Dto.Authentication;

public interface IAuthenticationDto
{
    string Email { get; }
    string Password { get; }
}
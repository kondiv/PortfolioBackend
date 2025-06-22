using System.ComponentModel.DataAnnotations;

public abstract class RegistrationRequest
{
    [Required, StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string EmailAddress { get; set; } = string.Empty;

    [Required, StringLength(32, MinimumLength = 8, ErrorMessage = "Длина пароля от 8 до 32 символов")]
    public string Password { get; set; } = string.Empty;

    [Required]
    public abstract string Role { get; }
}
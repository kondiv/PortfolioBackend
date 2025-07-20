using Domain.Dto.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Results;

namespace Services.Strategies.Registration;

public class UserRegistrationStrategy : IRegistrationStrategy
{
    private readonly EmployerRegistrationDto _employerRegistrationDto;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRegistrationStrategy(
        EmployerRegistrationDto employerRegistrationDto, 
        UserManager<ApplicationUser> userManager)
    {
        _employerRegistrationDto = employerRegistrationDto;
        _userManager = userManager;
    }
    
    public Task<RegistrationResult> Register(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
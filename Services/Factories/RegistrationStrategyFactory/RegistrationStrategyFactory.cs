using Domain.Dto.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using Services.Strategies.Registration;

namespace Services.Factories.RegistrationStrategyFactory;

public class RegistrationStrategyFactory : IRegistrationStrategyFactory
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserSkillService _userSkillService;
    
    public RegistrationStrategyFactory(
        UserManager<ApplicationUser> userManager,
        IUserSkillService userSkillService)
    {
        _userManager = userManager;
        _userSkillService = userSkillService;
    }
    
    public IRegistrationStrategy CreateDeveloperRegistrationStrategy(DeveloperRegistrationDto developerRegistrationDto)
    {
        return new DeveloperRegistrationStrategy(developerRegistrationDto, _userManager, _userSkillService);
    }

    public IRegistrationStrategy CreateUserRegistrationStrategy(EmployerRegistrationDto employerRegistrationDto)
    {
        throw new NotImplementedException();
    }
}
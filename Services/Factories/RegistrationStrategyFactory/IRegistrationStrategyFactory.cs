using Domain.Dto.Authentication;
using Services.Strategies.Registration;

namespace Services.Factories.RegistrationStrategyFactory;

public interface IRegistrationStrategyFactory
{
    IRegistrationStrategy CreateDeveloperRegistrationStrategy(DeveloperRegistrationDto developerRegistrationDto);
    IRegistrationStrategy CreateUserRegistrationStrategy(EmployerRegistrationDto employerRegistrationDto);
}
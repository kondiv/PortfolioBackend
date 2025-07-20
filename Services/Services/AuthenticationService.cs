using Data.Interfaces;
using Domain.Dto.Authentication;
using Microsoft.Extensions.Logging;
using Services.Factories.RegistrationStrategyFactory;
using Services.Interfaces;
using Services.Results;

namespace Services.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRegistrationStrategyFactory _registrationStrategyFactory;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<AuthenticationService> _logger;
    
    public AuthenticationService(
        IRegistrationStrategyFactory registrationStrategyFactory,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        ILogger<AuthenticationService> logger)
    {
        _registrationStrategyFactory = registrationStrategyFactory;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public async Task<RegistrationResult> RegisterDeveloperAsync(
        DeveloperRegistrationDto developerRegistrationDto, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Registering developer");
        
        var strategy = _registrationStrategyFactory.CreateDeveloperRegistrationStrategy(developerRegistrationDto);

        var result = await strategy.Register(cancellationToken);

        if (!result.Succeeded)
        {
            _logger.LogError("Failed to register developer");
        }
        else
        {
            _logger.LogInformation("Developer registered successfully");
        }
        
        return result;
    }

    public async Task<RegistrationResult> RegisterUserAsync(
        EmployerRegistrationDto employerRegistrationDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Registering user");
        
        var strategy = _registrationStrategyFactory.CreateUserRegistrationStrategy(employerRegistrationDto);
        
        var result = await strategy.Register(cancellationToken);

        if (result.Succeeded)
        {
            _logger.LogInformation("Registered user");
        }
        else
        {
            _logger.LogError("Failed to register user");
        }

        return result;
    }
}
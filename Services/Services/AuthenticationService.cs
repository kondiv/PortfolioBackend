using Data.Exceptions;
using Data.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Domain.Dto;
using Domain.Entities;
using Services.Interfaces;

namespace Services.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserSkillService _userSkillService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly PortfolioContext _context;

    public AuthenticationService(
        UserManager<ApplicationUser> userManager,
        IUserSkillService userSkillService,
        ITokenService tokenService,
        IMapper mapper,
        PortfolioContext context)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _context = context;
    }

    public async Task<IdentityResult> RegisterDeveloperAsync(
        DeveloperRegistrationDto developerRegistrationDto,
        CancellationToken cancellationToken = default)
    {
        var user = _mapper.Map<ApplicationUser>(developerRegistrationDto);
        
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        var result = await _userManager.CreateAsync(user, developerRegistrationDto.Password);
        
        if (!result.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            return result;
        }
        
        var addingSkillsResult = await _userSkillService.AddRangeAsync(user, developerRegistrationDto.Skills, cancellationToken);

        if (!addingSkillsResult.Succeeded)
        {
            await transaction.RollbackAsync(cancellationToken);
            var errors = addingSkillsResult.Errors.Select(e =>
                new IdentityError()
                {
                    Code = e.Code,
                    Description = e.Description
                }).ToArray();
            return IdentityResult.Failed(errors);
        }

        await transaction.CommitAsync(cancellationToken);
        
        return IdentityResult.Success;
    }

    public Task<IdentityResult> RegisterEmployerAsync(
        EmployerRegistrationDto employerRegistrationDto,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
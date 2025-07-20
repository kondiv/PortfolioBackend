using Domain.Dto;
using Domain.Dto.Authentication;
using Domain.Entities;
using Domain.SeedData;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using Services.Results;

namespace Services.Strategies.Registration;

public class DeveloperRegistrationStrategy : IRegistrationStrategy
{
    private readonly DeveloperRegistrationDto _developerRegistrationDto;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserSkillService _userSkillService;

    public DeveloperRegistrationStrategy(
        DeveloperRegistrationDto developerRegistrationDto,
        UserManager<ApplicationUser> userManager,
        IUserSkillService userSkillService)
    {
        _developerRegistrationDto = developerRegistrationDto;
        _userManager = userManager;
        _userSkillService = userSkillService;
    }
    
    public async Task<RegistrationResult> Register(CancellationToken cancellationToken = default)
    {
        var user = _developerRegistrationDto.Adapt<ApplicationUser>();
        
        var created = await _userManager.CreateAsync(user,  _developerRegistrationDto.Password);
        if (!created.Succeeded)
        {
            return RegistrationResult.Failed(created.Errors.Select(e => new Error(){Description = e.Description}).ToList());
        }

        var addedToRole = await _userManager.AddToRoleAsync(user, DefaultRoles.Developer);
        if (!addedToRole.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            return RegistrationResult.Failed(addedToRole.Errors.Select(e => new Error(){Description = e.Description}).ToList());
        }
        
        var skillsAdded = await _userSkillService.AddRangeAsync(user.Id, _developerRegistrationDto.Skills, cancellationToken);
        if (!skillsAdded.Succeeded)
        {
            await _userManager.DeleteAsync(user);
            
            return RegistrationResult.Failed(skillsAdded.Errors.ToList());
        }
        
        return RegistrationResult.Created(user.Adapt<UserDto>());
    }
}
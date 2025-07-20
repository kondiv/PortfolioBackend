using Domain.Dto;
using Domain.Dto.Authentication;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }
        
        [HttpPost("/developer/register")]
        public async Task<ActionResult<UserDto>> RegisterDeveloperAsync(
            DeveloperRegistrationDto developerRegistrationDto,
            CancellationToken cancellationToken = default)
        {
            var result = await _authenticationService.RegisterDeveloperAsync(developerRegistrationDto, cancellationToken);
            
            return !result.Succeeded
                ? BadRequest(result.Errors.Select(e => e.Description).ToArray())
                : StatusCode(201, result.User);
        }
    }
}

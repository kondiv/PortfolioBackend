using Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.Exceptions;
using Services.Interfaces;
using System.Runtime.InteropServices;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        public async Task<ActionResult<IEnumerable<Project>>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var projects = await _projectService.GetAllAsync(cancellationToken);
                return Ok(projects);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499);
            }
        }

        [HttpDelete("sremove/{projectId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        public async Task<ActionResult> RemoveAsync(
            Guid projectId,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await _projectService.RemoveAsync(projectId, cancellationToken);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499);
            }
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status499ClientClosedRequest)]
        public async Task<ActionResult> AddAsync(
            string title, 
            string description,
            string githubReference,
            CancellationToken cancellationToken = default)
        {
            var project = new Project()
            {
                Title = title,
                Description = description,
                GithubReference = githubReference
            };

            try
            {
                await _projectService.AddAsync(project, cancellationToken);
                return NoContent();
            }
            catch (InvalidModelException)
            {
                return BadRequest("Invalid data provided");
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

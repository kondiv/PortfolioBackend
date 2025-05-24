using Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Models.Exceptions;
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

        [HttpGet("projects/all")]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }

        [HttpDelete("project/remove/{projectId}")]
        public async Task<IActionResult> RemoveAsync(Guid projectId)
        {
            try
            {
                await _projectService.RemoveAsync(projectId);
                return NoContent();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("project/add")]
        public async Task<IActionResult> AddAsync(string title, string description, string githubReference)
        {
            var project = new Project()
            {
                Title = title,
                Description = description,
                GithubReference = githubReference
            };

            try
            {
                await _projectService.AddAsync(project);
                return NoContent();
            }
            catch (InvalidModelException)
            {
                return BadRequest("Invalid model provided");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

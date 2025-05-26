using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Domain.Entities;

namespace SkillSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectApiController : ControllerBase
    {
        private readonly IProjectApiService _projectService;

        public ProjectApiController(IProjectApiService projectService)
        {
            _projectService = projectService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectViewModel>>> GetProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectViewModel>> GetProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();
            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectViewModel>> CreateProject(ProjectViewModel model)
        {
            var project = await _projectService.CreateProjectAsync(model);
            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectViewModel model)
        {
            if (id != model.Id) return BadRequest();
            await _projectService.UpdateProjectAsync(model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return NoContent();
        }
    }
}

using SkillSync.Application.ViewModels;

namespace SkillSync.Web.Interfaces
{
    public interface IProjectWebService
    {
        Task<List<ProjectViewModel>> GetAllProjectsAsync();           // Get all projects
        Task<ProjectViewModel?> GetProjectByIdAsync(int id);          // Get project by ID
        Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel model); // Create new project
        Task UpdateProjectAsync(ProjectViewModel model);              // Update an existing project
        Task DeleteProjectAsync(int id);                               // Delete a project
    }
}

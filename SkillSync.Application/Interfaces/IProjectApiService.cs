using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillSync.Application.DTOs;
using SkillSync.Application.ViewModels;
using SkillSync.Domain.Entities;

namespace SkillSync.Application.Interfaces
{
    public interface IProjectApiService
    {
        Task<List<ProjectViewModel>> GetAllProjectsAsync();           // Get all projects
        Task<ProjectViewModel?> GetProjectByIdAsync(int id);          // Get project by ID
        Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel model); // Create new project
        Task UpdateProjectAsync(ProjectViewModel model);              // Update an existing project
        Task DeleteProjectAsync(int id);                               // Delete a project
    }
}

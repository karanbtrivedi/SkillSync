using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Domain.Entities;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Services
{
    public class ProjectApiService : IProjectService
    {
        private readonly HttpClient _httpClient;

        // Constructor to inject HttpClient
        public ProjectApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Get all projects from the API
        public async Task<List<ProjectViewModel>> GetAllProjectsAsync()
        {
            // Make GET request to fetch all projects from the API
            var response = await _httpClient.GetAsync("api/projectapi");
            response.EnsureSuccessStatusCode();  // Throws exception if response is unsuccessful

            // Read and deserialize the response content to a list of ProjectViewModel
            var content = await response.Content.ReadAsStringAsync();
            var projects = JsonSerializer.Deserialize<List<ProjectViewModel>>(content);

            // Return the list or an empty list if null
            return projects ?? new List<ProjectViewModel>();
        }

        // Get a project by its ID
        public async Task<ProjectViewModel?> GetProjectByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/projectapi/{id}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var project = JsonSerializer.Deserialize<ProjectViewModel>(content);

            return project;
        }

        // Create a new project
        public async Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel project)
        {
            var response = await _httpClient.PostAsJsonAsync("api/projectapi", project);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var createdProject = JsonSerializer.Deserialize<ProjectViewModel>(content);

            return createdProject;
        }

        // Update an existing project
        public async Task UpdateProjectAsync(ProjectViewModel project)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projectapi/{project.Id}", project);
            response.EnsureSuccessStatusCode();
        }

        // Delete a project by ID
        public async Task DeleteProjectAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/projectapi/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

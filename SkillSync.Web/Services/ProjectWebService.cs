using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Web.Interfaces;
using System.Text.Json;

namespace SkillSync.Web.Services
{
    public class ProjectWebService : IProjectWebService
    {
        private readonly HttpClient _httpClient;

        // Constructor to inject HttpClient
        public ProjectWebService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:7147/");
        }

        // Get all projects from the API
        public async Task<List<ProjectViewModel>> GetAllProjectsAsync()
        {
            var response = await _httpClient.GetAsync("api/projectapi");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProjectViewModel>>(content) ?? new List<ProjectViewModel>();
        }

        // Get a project by its ID
        public async Task<ProjectViewModel?> GetProjectByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/projectapi/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProjectViewModel>(content);
        }

        // Create a new project
        public async Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/projectapi", model);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProjectViewModel>(content);
        }

        // Update an existing project
        public async Task UpdateProjectAsync(ProjectViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projectapi/{model.Id}", model);
            response.EnsureSuccessStatusCode();
        }

        // Delete a project by its ID
        public async Task DeleteProjectAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/projectapi/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

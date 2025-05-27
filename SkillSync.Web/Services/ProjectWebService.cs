using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Web.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SkillSync.Web.Services
{
    public class ProjectWebService : IProjectWebService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor to inject HttpClient and IHttpContextAccessor
        public ProjectWebService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            // Get token from cookie and attach to Authorization header
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwttoken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<ProjectViewModel>> GetAllProjectsAsync()
        {
            var response = await _httpClient.GetAsync("api/projectapi");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProjectViewModel>>(content) ?? new List<ProjectViewModel>();
        }

        public async Task<ProjectViewModel?> GetProjectByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/projectapi/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProjectViewModel>(content);
        }

        public async Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/projectapi", model);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProjectViewModel>(content);
        }

        public async Task UpdateProjectAsync(ProjectViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projectapi/{model.Id}", model);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProjectAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/projectapi/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

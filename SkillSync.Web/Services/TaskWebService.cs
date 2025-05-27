using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Web.Interfaces;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace SkillSync.Web.Services
{
    public class TaskWebService : ITaskWebService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TaskWebService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            // Add JWT token from cookie to Authorization header
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwttoken"];
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<TaskViewModel>> GetAllTasksAsync()
        {
            var response = await _httpClient.GetAsync("api/taskapi");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TaskViewModel>>(content) ?? new List<TaskViewModel>();
        }

        public async Task<TaskViewModel?> GetTaskByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/taskapi/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TaskViewModel>(content);
        }

        public async Task<TaskViewModel> CreateTaskAsync(TaskViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/taskapi", model);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TaskViewModel>(content);
        }

        public async Task UpdateTaskAsync(TaskViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/taskapi/{model.Id}", model);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/taskapi/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

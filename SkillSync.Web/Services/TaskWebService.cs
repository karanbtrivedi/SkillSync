using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Web.Interfaces;
using System.Text.Json;

namespace SkillSync.Web.Services
{
    public class TaskWebService : ITaskWebService
    {
        private readonly HttpClient _httpClient;

        // Constructor to inject HttpClient
        public TaskWebService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            //_httpClient.BaseAddress = new Uri("https://localhost:7147/");
        }

        // Get all tasks from the API
        public async Task<List<TaskViewModel>> GetAllTasksAsync()
        {
            var response = await _httpClient.GetAsync("api/taskapi");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TaskViewModel>>(content) ?? new List<TaskViewModel>();
        }

        // Get a task by its ID
        public async Task<TaskViewModel?> GetTaskByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/taskapi/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TaskViewModel>(content);
        }

        // Create a new task
        public async Task<TaskViewModel> CreateTaskAsync(TaskViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/taskapi", model);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TaskViewModel>(content);
        }

        // Update an existing task
        public async Task UpdateTaskAsync(TaskViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/taskapi/{model.Id}", model);
            response.EnsureSuccessStatusCode();
        }

        // Delete a task by its ID
        public async Task DeleteTaskAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/taskapi/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}

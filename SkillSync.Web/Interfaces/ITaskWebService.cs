using SkillSync.Application.ViewModels;

namespace SkillSync.Web.Interfaces
{
    public interface ITaskWebService
    {
        Task<List<TaskViewModel>> GetAllTasksAsync();  // Retrieve all tasks
        Task<TaskViewModel?> GetTaskByIdAsync(int id);  // Retrieve a task by ID
        Task<TaskViewModel> CreateTaskAsync(TaskViewModel model);  // Create a new task
        Task UpdateTaskAsync(TaskViewModel task);  // Update an existing task
        Task DeleteTaskAsync(int id);  // Delete a task by ID
    }
}

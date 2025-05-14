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
    public interface ITaskApiService
    {
        Task<List<TaskViewModel>> GetAllTasksAsync();  // Retrieve all tasks
        Task<TaskViewModel?> GetTaskByIdAsync(int id);  // Retrieve a task by ID
        Task<TaskViewModel> CreateTaskAsync(TaskViewModel model);  // Create a new task
        Task UpdateTaskAsync(TaskViewModel task);  // Update an existing task
        Task DeleteTaskAsync(int id);  // Delete a task by ID
    }
}

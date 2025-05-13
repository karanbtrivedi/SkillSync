using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Domain.Entities;
using SkillSync.Domain.Enums;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Services
{
    public class TaskApiService : ITaskService
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskApiService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TaskViewModel>> GetAllTasksAsync()
        {
            var tasks = await _dbContext.Tasks.Include(t => t.Project).ToListAsync();
            return tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                DueDate = t.DueDate,
                CreatedDate = t.CreatedDate,
                ProjectId = t.ProjectId
            }).ToList();
        }

        public async Task<TaskViewModel?> GetTaskByIdAsync(int id)
        {
            var task = await _dbContext.Tasks.Include(t => t.Project).FirstOrDefaultAsync(t => t.Id == id);
            if (task == null)
            {
                return null;
            }
            return new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                CreatedDate = task.CreatedDate,
                ProjectId = task.ProjectId
            };
        }

        public async Task<TaskViewModel> CreateTaskAsync(TaskViewModel model)
        {
            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                DueDate = model.DueDate,
                CreatedDate = DateTime.Now,
                ProjectId = model.ProjectId
            };

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                CreatedDate = task.CreatedDate,
                ProjectId = task.ProjectId
            };
        }

        public async Task UpdateTaskAsync(TaskViewModel model)
        {
            var task = await _dbContext.Tasks.FindAsync(model.Id);
            if (task != null)
            {
                task.Title = model.Title;
                task.Description = model.Description;
                task.Status = model.Status;
                task.DueDate = model.DueDate;
                task.ProjectId = model.ProjectId;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _dbContext.Tasks.FindAsync(id);
            if (task != null)
            {
                _dbContext.Tasks.Remove(task);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}

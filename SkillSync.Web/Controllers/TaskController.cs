using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkillSync.Application.DTOs;
using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Domain.Entities;
using SkillSync.Web.Interfaces;

namespace SkillSync.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskWebService _taskService;
        private readonly IProjectWebService _projectService;

        public TaskController(ITaskWebService taskService, IProjectWebService projectService)
        {
            _taskService = taskService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            var viewModel = tasks.Select(task => new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                ProjectName = task.ProjectName  // Assuming Project is loaded in Task model
            }).ToList();

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            var viewModel = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                ProjectName = task.ProjectName
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetAllProjectsAsync();

            var viewModel = new TaskViewModel
            {
                DueDate = DateTime.Today.AddDays(1),
                Projects = projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var projects = await _projectService.GetAllProjectsAsync();
                model.Projects = projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();

                return View(model);
            }

            await _taskService.CreateTaskAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            var projects = await _projectService.GetAllProjectsAsync();

            var viewModel = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                ProjectId = task.ProjectId,
                Projects = projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var projects = await _projectService.GetAllProjectsAsync();
                model.Projects = projects.Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name
                }).ToList();
                return View(model);
            }

            var task = await _taskService.GetTaskByIdAsync(model.Id);
            if (task == null) return NotFound();

            task.Title = model.Title;
            task.Description = model.Description;
            task.Status = model.Status;
            task.DueDate = model.DueDate;
            task.ProjectId = model.ProjectId;

            await _taskService.UpdateTaskAsync(task);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();

            var viewModel = new TaskViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                DueDate = task.DueDate,
                ProjectName = task.ProjectName
            };

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteTaskAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}

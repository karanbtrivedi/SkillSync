using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SkillSync.Application.Interfaces;
using SkillSync.Application.Services;
using SkillSync.Domain.Entities;
using SkillSync.Web.ViewModels;

namespace SkillSync.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public TaskController(ITaskService taskService, IProjectService projectService)
        {
            _taskService = taskService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();

            var viewModel = tasks.Select(t => new TaskViewModel
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
                DueDate = t.DueDate,
                ProjectName = t.Project?.Name
            });

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
                ProjectName = task.Project?.Name
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var projects = await _projectService.GetAllProjectsAsync();

            var viewModel = new TaskViewModel
            {
                DueDate = DateTime.Today.AddDays(1),
                Projects = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
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
                model.Projects = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
                return View(model);
            }

            var task = new TaskItem
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                DueDate = model.DueDate,
                ProjectId = model.ProjectId
            };

            await _taskService.CreateTaskAsync(task);
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
                Projects = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
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
                model.Projects = projects.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
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
                ProjectName = task.Project?.Name
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

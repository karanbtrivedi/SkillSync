using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Web.Interfaces;

namespace SkillSync.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectWebService _projectWebService;

        public ProjectController(IProjectWebService projectWebService)
        {
            _projectWebService = projectWebService;
        }

        [AllowAnonymous]
        // GET: /Project
        public async Task<IActionResult> Index()
        {
            var projects = await _projectWebService.GetAllProjectsAsync();
            return View(projects);
        }

        // GET: /Project/Details/5
        [Authorize(AuthenticationSchemes = "SmartScheme")]
        //[AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectWebService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        // GET: /Project/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View(new ProjectViewModel
            {
                StartDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(7)
            });
        }

        // POST: /Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _projectWebService.CreateProjectAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Project/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectWebService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        // POST: /Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ProjectViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            await _projectWebService.UpdateProjectAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Project/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectWebService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        // POST: /Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectWebService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}

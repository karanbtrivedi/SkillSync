using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SkillSync.Application.Interfaces;
using SkillSync.Application.ViewModels;
using SkillSync.Domain.Entities;
using SkillSync.Infrastructure.Data;

namespace SkillSync.Infrastructure.Services
{
    public class ProjectApiService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject ApplicationDbContext
        public ProjectApiService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all projects from the database
        public async Task<List<ProjectViewModel>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();

            return projects;
        }

        // Get a project by its ID from the database
        public async Task<ProjectViewModel?> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .Where(p => p.Id == id)
                .Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .FirstOrDefaultAsync();

            return project;
        }

        // Create a new project
        public async Task<ProjectViewModel> CreateProjectAsync(ProjectViewModel project)
        {
            var newProject = new Project
            {
                Name = project.Name
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();

            return new ProjectViewModel
            {
                Id = newProject.Id,
                Name = newProject.Name
            };
        }

        // Update an existing project
        public async Task UpdateProjectAsync(ProjectViewModel project)
        {
            var existingProject = await _context.Projects.FindAsync(project.Id);
            if (existingProject != null)
            {
                existingProject.Name = project.Name;
                await _context.SaveChangesAsync();
            }
        }

        // Delete a project by ID
        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }
    }
}

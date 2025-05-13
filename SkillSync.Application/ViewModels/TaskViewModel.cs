using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore;
using SkillSync.Application.DTOs;
using SkillSync.Domain.Enums;

namespace SkillSync.Application.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }  // Unique identifier for the task

        public string Title { get; set; } = string.Empty;  // Title of the task

        public string? Description { get; set; }  // Detailed description

        public int ProjectId { get; set; }  // Foreign key to the project

        public string? ProjectName { get; set; }  // Optional project name for display

        public DateTime DueDate { get; set; }  // Due date for task

        public Domain.Enums.TaskStatus Status { get; set; } = Domain.Enums.TaskStatus.Pending;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;  // Creation date

        // ✅ Dropdown support for project list in Create/Edit forms
        public List<ProjectDto>? Projects { get; set; }
    }
}

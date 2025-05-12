using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillSync.Domain.Entities
{
    public class TaskItem
    {
        public int Id { get; set; }  // Primary key
        public string Title { get; set; } = string.Empty;  // Title of the task
        public string? Description { get; set; }  // Optional description
        public bool IsCompleted { get; set; } = false;  // Task completion status
        public DateTime DueDate { get; set; }  // Task due date

        // Foreign key reference to the Project entity
        public int ProjectId { get; set; }

        // Navigation property to the Project entity
        public Project Project { get; set; } = default!;
    }
}

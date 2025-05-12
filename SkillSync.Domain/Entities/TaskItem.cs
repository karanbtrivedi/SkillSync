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
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;
        public DateTime DueDate { get; set; }

        // Foreign key
        public int ProjectId { get; set; }

        // Navigation property
        public Project Project { get; set; } = default!;
    }
}

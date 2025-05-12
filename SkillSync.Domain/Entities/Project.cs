using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillSync.Domain.Entities
{
    public class Project
    {
        public int Id { get; set; }  // Primary key
        public string Name { get; set; } = string.Empty;  // Project name (required)
        public string? Description { get; set; }  // Optional description
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Timestamp

        // Navigation property to related Task entities
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}

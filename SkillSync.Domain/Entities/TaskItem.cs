using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using SkillSync.Domain.Enums;

namespace SkillSync.Domain.Entities
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Pending;

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        public Project? Project { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}

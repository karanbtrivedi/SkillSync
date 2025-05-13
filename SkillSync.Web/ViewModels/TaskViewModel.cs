using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace SkillSync.Web.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public Domain.Enums.TaskStatus Status { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        public int ProjectId { get; set; }

        public string? ProjectName { get; set; }

        // For dropdown
        public List<SelectListItem>? Projects { get; set; }
    }
}

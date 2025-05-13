using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillSync.Application.DTOs
{
    public class ProjectDto
    {
        public int Id { get; set; }  // For Update, Get
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}

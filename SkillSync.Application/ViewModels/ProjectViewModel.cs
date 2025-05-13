using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillSync.Application.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }  // Unique identifier for the project
        public string Name { get; set; }  // Name of the project
        public string Description { get; set; }  // Detailed description of the project
        public DateTime CreatedAt { get; set; }  // Date when the project was created
    }
}

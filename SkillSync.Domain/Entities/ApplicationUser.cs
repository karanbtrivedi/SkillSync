using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SkillSync.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Add any custom fields here
        public string? FullName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillSync.Application.ViewModels
{
    public class ApiLoginViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SkillSync.Web.Controllers
{
    public class HomeController : Controller
    {
        // Homepage (anyone can view)
        public IActionResult Index()
        {
            return View();
        }

        // Optional: Error page
        public IActionResult Error()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Application.ViewModels;
using SkillSync.Domain.Entities;
using SkillSync.Infrastructure.Services;

namespace SkillSync.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtTokenService _tokenService;

        public AuthController(UserManager<ApplicationUser> userManager, JwtTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = _tokenService.GenerateToken(user, roles);
                return Ok(new { token });
            }

            return Unauthorized();
        }
    }
}

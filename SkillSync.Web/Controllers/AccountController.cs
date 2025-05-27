using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillSync.Domain.Entities;  // Assuming ApplicationUser is here
using SkillSync.Web.Models;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SkillSync.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                IHttpClientFactory httpClientFactory)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpClientFactory = httpClientFactory;
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            await _userManager.AddToRoleAsync(user, "User");

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Add errors to ModelState to show in the view
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Optional: Clear existing auth token cookie/session
            if (Request.Cookies.ContainsKey("jwt_token"))
            {
                Response.Cookies.Delete("jwt_token");
            }

            return View(new LoginViewModel());
        }


        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = _httpClientFactory.CreateClient();
            var loginPayload = new
            {
                UserName = model.Email,
                Password = model.Password
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(loginPayload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7147/api/auth/login", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                var token = doc.RootElement.GetProperty("token").GetString();

                if (!string.IsNullOrEmpty(token))
                {
                    // Decode JWT manually to extract role
                    var jwtHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var jwtToken = jwtHandler.ReadJwtToken(token);

                    var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "User";
                    var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value ?? model.Email;

                    // Add claims to Cookie auth identity
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, nameClaim),
                new Claim(ClaimTypes.Role, roleClaim) // 👈 This enables role-based auth
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Store JWT token in cookie for API calls
                    HttpContext.Response.Cookies.Append("JwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddHours(1)
                    });

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            // Remove JWT cookie on logout
            Response.Cookies.Delete("JwtToken");
            return RedirectToAction("Index", "Home");
        }
    }
}

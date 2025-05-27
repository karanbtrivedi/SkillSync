using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SkillSync.Application.Interfaces;
using SkillSync.Domain.Entities;
using SkillSync.Domain.Repositories;
using SkillSync.Infrastructure.Data;
using SkillSync.Infrastructure.Repositories;
using SkillSync.Infrastructure.Services;
using SkillSync.Web.Interfaces;
using SkillSync.Web.Middleware;
using SkillSync.Web.Services;
using System.Security.Claims;
using System.Text;

namespace SkillSync.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = "SmartScheme"; // custom scheme
            options.DefaultChallengeScheme = "SmartScheme";
        }).AddPolicyScheme("SmartScheme", "Authorization Bearer or Cookies", options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                var path = context.Request.Path;
                if (path.StartsWithSegments("/api")) // API calls use JWT Bearer
                    return JwtBearerDefaults.AuthenticationScheme;
                else
                    return CookieAuthenticationDefaults.AuthenticationScheme; // MVC uses cookies
            };
        }).AddCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                NameClaimType = ClaimTypes.Name
            };
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["JwtToken"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
        });

        //builder.Services.AddAuthentication(options =>
        //{
        //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        //}).AddJwtBearer(options =>
        //{
        //    options.TokenValidationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuer = true,
        //        ValidateAudience = true,
        //        ValidateLifetime = true,
        //        ValidateIssuerSigningKey = true,
        //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        //        ValidAudience = builder.Configuration["Jwt:Audience"],
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

        //        // **Important: Map role claims**
        //        RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
        //        NameClaimType = ClaimTypes.Name
        //    };
        //    options.Events = new JwtBearerEvents
        //    {
        //        OnMessageReceived = context =>
        //        {
        //            // Read token from cookie
        //            var token = context.Request.Cookies["JwtToken"];
        //            if (!string.IsNullOrEmpty(token))
        //            {
        //                context.Token = token;
        //            }
        //            return Task.CompletedTask;
        //        }
        //    };
        //})
        //.AddCookie(options =>
        //{
        //    options.LoginPath = "/Account/Login";
        //    options.AccessDeniedPath = "/Account/AccessDenied";
        //    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        //    options.SlidingExpiration = true;
        //});


        // Register HttpClient for Project and Task services
        builder.Services.AddHttpClient<IProjectWebService, ProjectWebService>(client =>
{
client.BaseAddress = new Uri("https://localhost:7147/"); // Update with the actual base URL of your API
client.DefaultRequestHeaders.Add("Accept", "application/json");
});

        builder.Services.AddHttpClient<ITaskWebService, TaskWebService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7147/"); // Update with the actual base URL of your API
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<ITaskRepository, TaskRepository>();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
            options.AccessDeniedPath = "/Account/AccessDenied";
        });

        var app = builder.Build();

        // Seed roles
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            SeedRolesAndAdminAsync(roleManager, userManager);
        }

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<JwtCookieAuthenticationMiddleware>();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();

        static async Task SeedRolesAndAdminAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Optional: Create a default Admin user
            var adminEmail = "admin@skillsync.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "SkillSync Admin",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
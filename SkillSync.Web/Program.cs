using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillSync.Application.Interfaces;
using SkillSync.Domain.Entities;
using SkillSync.Domain.Repositories;
using SkillSync.Infrastructure.Data;
using SkillSync.Infrastructure.Repositories;
using SkillSync.Infrastructure.Services;
using SkillSync.Web.Interfaces;
using SkillSync.Web.Services;

namespace SkillSync.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();



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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}

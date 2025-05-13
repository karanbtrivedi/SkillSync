using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SkillSync.Application.Interfaces;
using SkillSync.Domain.Entities;
using SkillSync.Domain.Repositories;
using SkillSync.Infrastructure.Data;
using SkillSync.Infrastructure.Repositories;
using SkillSync.Infrastructure.Services;

namespace SkillSync.Web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        // Register TaskApiService and ITaskService in DI container
        builder.Services.AddHttpClient<TaskApiService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:5001");  // Replace with your actual API URL
        });

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IProjectService, ProjectApiService>();

        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ITaskService, TaskApiService>();

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
            pattern: "{controller=Task}/{action=Index}/{id?}");

        app.Run();
    }
}

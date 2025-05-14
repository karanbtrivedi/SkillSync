
using Microsoft.EntityFrameworkCore;
using SkillSync.Application.Interfaces;
using SkillSync.Domain.Repositories;
using SkillSync.Infrastructure.Data;
using SkillSync.Infrastructure.Repositories;
using SkillSync.Infrastructure.Services;

namespace SkillSync.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Keep C# casing
                });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
        builder.Services.AddScoped<IProjectApiService, ProjectApiService>();

        builder.Services.AddScoped<ITaskRepository, TaskRepository>();
        builder.Services.AddScoped<ITaskApiService, TaskApiService>();

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

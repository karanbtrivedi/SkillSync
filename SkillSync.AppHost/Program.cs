var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SkillSync_Web>("skillsync-web");

builder.AddProject<Projects.SkillSync_API>("skillsync-api");

builder.Build().Run();

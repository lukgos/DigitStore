using DigitStore.Api;
using Shared.EntityFramework;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddModules(builder.Configuration);
builder.Services.AddInfrastructureServices(builder.Configuration, ModulesConfiguration.Assemblies);
builder.Services.AddEntityFrameworkServices();

var app = builder.Build();

app.UseInfrastructureServices();

app.MapModuleEndpoints();

await app.ApplyMigrationsAsync();

app.Run();
using Account.Module;
using DigitStore.Api;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAccount(builder.Configuration);
builder.Services.AddInfrastructureService();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAccountModuleEndpoints();
await app.ApplyMigrationsAsync();

app.Run();
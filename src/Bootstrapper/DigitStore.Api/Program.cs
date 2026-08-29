using Account.Module;
using Catalog.Module;
using DigitStore.Api;
using Search.Module;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAccount(builder.Configuration);
builder.Services.AddCatalog(builder.Configuration);
builder.Services.AddSearchModule(builder.Configuration);
builder.Services.AddInfrastructureServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAccountModuleEndpoints();
app.MapCatalogModuleEndpoints();
app.MapSearchModuleEndpoints();

await app.ApplyMigrationsAsync();

app.Run();
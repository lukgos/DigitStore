using Account.Module;
using Catalog.Module;
using DigitStore.Api;
using Order.Module;
using Search.Module;
using Shared.EntityFramework;
using Shared.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization();
builder.Services.AddAccountModule(builder.Configuration);
builder.Services.AddCatalogModule(builder.Configuration);
builder.Services.AddSearchModule(builder.Configuration);
builder.Services.AddOrderModule(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddEntityFrameworkServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapAccountModuleEndpoints();
app.MapCatalogModuleEndpoints();
app.MapSearchModuleEndpoints();
app.MapOrderModuleEndpoints();

await app.ApplyMigrationsAsync();

app.Run();
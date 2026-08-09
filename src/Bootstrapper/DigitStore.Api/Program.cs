using Account.Module;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAccount(builder.Configuration);

var app = builder.Build();


app.Run();
using System.Text;
using Account.Module.Abstractions;
using Account.Module.DAL;
using Account.Module.DAL.Repositories;
using Account.Module.Entities;
using Account.Module.Features.GetMe;
using Account.Module.Features.GetUser;
using Account.Module.Features.SignIn;
using Account.Module.Features.SignUp;
using Account.Module.Services;
using Account.Module.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Shared.Application;

namespace Account.Module;

public static class Extensions
{
    public static IServiceCollection AddAccount(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddModuleServices(typeof(Extensions).Assembly);
        
        services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
        
        var connectionString = configuration.GetConnectionString("Postgres");
        
        services.AddDbContext<AccountDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        
        services
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddSingleton<ITokenManager, JwtTokenManager>()
            .AddScoped<ITokenStorage, TokenStorage>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        
        var jwtOptions = configuration
            .GetSection("JwtOptions")
            .Get<JwtOptions>()!;

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];
                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();
        
        return services;
    }
    
    public static IEndpointRouteBuilder MapAccountModuleEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSignUpEndpoint();
        app.MapSignInEndpoint();
        app.MapGetUserEndpoint();
        app.MapGetMeEndpoint();

        return app;
    }
}
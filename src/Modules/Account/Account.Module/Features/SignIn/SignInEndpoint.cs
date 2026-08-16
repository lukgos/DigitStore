using Account.Module.Abstractions;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.SignIn;

public static class SignInEndpoint
{
    public static void MapSignInEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/sign-in", async (SignInCommand command,
                [FromServices] ICommandHandler<SignInCommand> handler,
                [FromServices] IValidator<SignInCommand> validator,
                [FromServices] ITokenStorage tokenStorage,
                HttpContext httpContext,
                CancellationToken cancellationToken) =>
            {
                var validationResult = await validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                await handler.HandleAsync(command, cancellationToken);

                var accessToken = tokenStorage.Get();

                httpContext.Response.Cookies.Append("access_token", accessToken,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                    });

                return Results.Ok();
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);
    }
}
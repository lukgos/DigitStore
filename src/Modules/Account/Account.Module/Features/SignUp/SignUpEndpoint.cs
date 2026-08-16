using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.SignUp;

public static class SignUpEndpoint
{
    public static void MapSignUpEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/account/sign-up", async (SignUpCommand command, 
                [FromServices] ICommandHandler<SignUpCommand> handler,
                [FromServices] IValidator<SignUpCommand> validator,
                CancellationToken cancellationToken) =>
            {
                
                var userId = Guid.NewGuid();

                var signUpCommand = new SignUpCommand(userId, command.Email, command.Password);
            
                var validationResult = await validator.ValidateAsync(signUpCommand, cancellationToken);
            
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                
                await handler.HandleAsync(signUpCommand, cancellationToken);
            
                return Results.CreatedAtRoute("GetUserRoute", new { id = userId }, null);
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest);;
    }
}
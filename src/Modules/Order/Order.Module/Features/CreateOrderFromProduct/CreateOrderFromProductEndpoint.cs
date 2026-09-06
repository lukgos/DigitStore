using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.Auth;
using Shared.Abstractions.CQRS;

namespace Order.Module.Features.CreateOrderFromProduct;

public record CreateOrderFromProductRequest(Guid ProductId, int Quantity);

public static class CreateOrderFromProductEndpoint
{
    public static void MapCreateOrderFromProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/orders/from-product", [Authorize] async (
                CreateOrderFromProductRequest request, 
                [FromServices] IUserContext userContext,
                [FromServices] ICommandHandler<CreateOrderFromProductCommand> handler,
                [FromServices] IValidator<CreateOrderFromProductCommand> validator,
                CancellationToken ct) =>
            {
                var orderId = Guid.NewGuid();

                var customerId = userContext.UserId;
                
                if (customerId is null)
                {
                    return Results.Unauthorized();
                }

                var command = new CreateOrderFromProductCommand(
                    orderId, 
                    customerId.Value, 
                    request.ProductId, 
                    request.Quantity);
            
                var validationResult = await validator.ValidateAsync(command, ct);
            
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                
                await handler.HandleAsync(command, ct);
            
                return Results.Created($"/api/orders/{orderId}", null);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}
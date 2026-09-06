using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Order.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Order.Module.Features.GetOrderById;

public static class GetOrderByIdEndpoint
{
    public static void MapGetOrderByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/orders/{id:guid}", async (Guid id, 
                [FromServices] IQueryHandler<GetOrderByIdQuery, OrderDetailsDto?> handler,
                [FromServices] IValidator<GetOrderByIdQuery> validator,
                CancellationToken ct) =>
            {
                var query = new GetOrderByIdQuery(id);
                
                var validationResult = await validator.ValidateAsync(query, ct);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                var order = await handler.HandleAsync(query, ct);
                
                if (order is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(order);
            })
            .RequireAuthorization()
            .Produces<OrderDetailsDto>(StatusCodes.Status200OK)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound);
    }
}
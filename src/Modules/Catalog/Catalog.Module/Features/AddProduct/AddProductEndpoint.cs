using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.AddProduct;

public static class AddProductEndpoint
{
    public static void MapAddProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/catalog/products", [Authorize(Roles = "admin")] async (AddProductRequest request, 
                [FromServices] ICommandHandler<AddProductCommand> handler,
                [FromServices] IValidator<AddProductCommand> validator,
                CancellationToken ct) =>
            {
                var productId = Guid.NewGuid();
                var command = new AddProductCommand(
                    productId, 
                    request.Name, 
                    request.Description, 
                    request.Price, 
                    request.CategoryId,
                    request.Tags ?? Enumerable.Empty<string>(),
                    request.Images ?? Enumerable.Empty<ProductImageCommand>(),
                    request.Attributes ?? new Dictionary<string, string>()
                );
            
                var validationResult = await validator.ValidateAsync(command, ct);
            
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                
                await handler.HandleAsync(command, ct);
            
                return Results.CreatedAtRoute("GetProductRoute", new { id = productId }, null);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}
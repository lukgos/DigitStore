using Catalog.Module.Features.AddProduct;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.UpdateProduct;

public static class UpdateProductEndpoint
{
    public static void MapUpdateProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPut("/api/catalog/products/{id:guid}", [Authorize(Roles = "admin")] async (Guid id, UpdateProductRequest request,
                [FromServices] ICommandHandler<UpdateProductCommand> handler,
                [FromServices] IValidator<UpdateProductCommand> validator,
                CancellationToken ct) =>
            {
                var command = new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.CategoryId, 
                    request.Tags ?? Enumerable.Empty<string>(), request.Images ?? Enumerable.Empty<ProductImageCommand>(), 
                    request.Attributes ?? new Dictionary<string, string>());
            
                var validationResult = await validator.ValidateAsync(command, ct);
            
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                
                await handler.HandleAsync(command, ct);
            
                return Results.NoContent();
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);
    }
}
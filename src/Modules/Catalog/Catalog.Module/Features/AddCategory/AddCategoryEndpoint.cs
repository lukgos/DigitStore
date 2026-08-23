using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.AddCategory;

public static class AddCategoryEndpoint
{
    public static void MapAddCategoryEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/catalog/categories", [Authorize(Roles = "admin")] async (AddCategoryRequest request, 
                [FromServices] ICommandHandler<AddCategoryCommand> handler,
                [FromServices] IValidator<AddCategoryCommand> validator,
                CancellationToken ct) =>
            {
                var categoryId = Guid.NewGuid();
                var command = new AddCategoryCommand(categoryId, request.Name, request.Description);
            
                var validationResult = await validator.ValidateAsync(command, ct);
            
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
                
                await handler.HandleAsync(command, ct);
            
                return Results.Created($"/api/catalog/categories/{categoryId}", null);
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized);
    }
}

public record AddCategoryRequest(string Name, string Description);


using Catalog.Module.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.GetProduct;

public static class GetProductEndpoint
{
    public static void MapGetProductEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/catalog/products/{id:guid}", async (Guid id, HttpContext httpContext,
                [FromServices] IQueryHandler<GetProductQuery, ProductDto?> handler,
                CancellationToken ct) =>
            {
                var query = new GetProductQuery(id);
                var product = await handler.HandleAsync(query, ct);
                
                if (product is null)
                {
                    return Results.NotFound();
                }
                
                return Results.Ok(product);
            })
            .WithName("GetProductRoute")
            .Produces<ProductDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status304NotModified)
            .Produces(StatusCodes.Status404NotFound);
    }
}
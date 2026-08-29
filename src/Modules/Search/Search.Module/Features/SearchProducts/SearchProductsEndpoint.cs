using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Search.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Search.Module.Features.SearchProducts;

public static class SearchProductsEndpoint
{
    public static void MapSearchProductsEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/search/products", async (
                [AsParameters] SearchProductsQuery query,
                [FromServices] IQueryHandler<SearchProductsQuery, SearchPagedResponseDto> handler,
                CancellationToken ct) =>
            {
                var result = await handler.HandleAsync(query, ct);
                
                return Results.Ok(result);
            })
            .WithName("SearchProductsRoute")
            .Produces<SearchPagedResponseDto>(StatusCodes.Status200OK);
    }
}
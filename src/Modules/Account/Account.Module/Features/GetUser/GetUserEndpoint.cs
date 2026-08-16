using Account.Module.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.GetUser;

public static class GetUserEndpoint
{
    public static void MapGetUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/account/{id:guid}", async (Guid id,
                [FromServices] IQueryHandler<GetUserQuery, UserDto> handler,
                CancellationToken cancellationToken) =>
            {
                var query = new GetUserQuery(id);
                var user = await handler.HandleAsync(query, cancellationToken);

                return user is not null ? Results.Ok(user) : Results.NotFound();
            })
            .WithName("GetUserRoute")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);
    }
}
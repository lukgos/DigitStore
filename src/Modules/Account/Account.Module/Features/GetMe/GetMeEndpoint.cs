using Account.Module.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Shared.Abstractions.Auth;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.GetMe;

public static class GetMeEndpoint
{
    public static void MapGetMeEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/account/me", async ([FromServices] IUserContext userContext, 
                [FromServices] IQueryHandler<GetMeQuery, UserDto?> handler,
                CancellationToken cancellationToken) =>
            {
                if (userContext.UserId is null)
                {
                    return Results.Unauthorized();
                }
                
                var query = new GetMeQuery(userContext.UserId.Value);

                var user = await handler.HandleAsync(query, cancellationToken);

                return user is null ? Results.NotFound() : Results.Ok(user);
            })
            .RequireAuthorization()
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status404NotFound);
    }
}
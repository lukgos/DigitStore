using Account.Module.DAL;
using Account.Module.DTOs;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Account.Module.Features.GetMe;

public sealed class GetMeQueryHandler(AccountDbContext dbContext)
    : IQueryHandler<GetMeQuery, UserDto>
{
    public async Task<UserDto?> HandleAsync(
        GetMeQuery query,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(query.UserId);
        
        return await dbContext.Users
            .AsNoTracking()
            .Where(x => x.Id == userId)
            .Select(x => new UserDto(
                x.Id.Value,
                x.Email.Value,
                x.Role.Value))
            .SingleOrDefaultAsync(cancellationToken);
    }
}
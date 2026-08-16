using Account.Module.DAL;
using Account.Module.DTOs;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.CQRS;

namespace Account.Module.Features.GetUser;

public sealed class GetUserQueryHandler(AccountDbContext dbContext) : IQueryHandler<GetUserQuery, UserDto?>
{
    public async Task<UserDto?> HandleAsync(GetUserQuery query, CancellationToken cancellationToken)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .Where(x => x.Id.Value == query.Id)
            .Select(x => new UserDto(x.Id.Value, x.Email.Value, x.Role.Value))
            .SingleOrDefaultAsync(cancellationToken);

        return user;
    }
}
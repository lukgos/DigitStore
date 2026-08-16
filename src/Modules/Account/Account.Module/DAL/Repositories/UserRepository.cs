using Account.Module.Entities;
using Account.Module.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Account.Module.DAL.Repositories;

internal sealed class UserRepository(AccountDbContext dbContext) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await dbContext.Users.AnyAsync(x => x.Email == email, cancellationToken);
    }
    
    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken)
    {
        return await dbContext.Users.SingleOrDefaultAsync(x => x.Email == email, cancellationToken);
    }
}
using Catalog.Module.Entities;
using Catalog.Module.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.DAL.Repositories;

public sealed class CategoryRepository(CatalogDbContext dbContext) : ICategoryRepository
{
    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct)
    {
        return dbContext.Categories.AnyAsync(c => c.Name == name && c.IsDeleted == false, ct);
    }

    public Task<Category?> GetByIdAsync(CategoryId id, CancellationToken ct)
    {
        return dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public Task<Category?> GetByNameAsync(string name, CancellationToken ct)
    {
        return dbContext.Categories.FirstOrDefaultAsync(c => c.Name == name, ct);
    }
    

    public async Task AddAsync(Category category, CancellationToken ct)
    {
        await dbContext.Categories.AddAsync(category, ct);
    }
    
    public void Delete(Category category, CancellationToken ct)
    {
        dbContext.Categories.Remove(category);
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}
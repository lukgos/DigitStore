using Catalog.Module.Entities;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Repositories;

public interface ICategoryRepository
{
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task<Category?> GetByIdAsync(CategoryId id, CancellationToken ct);
    Task<Category?> GetByNameAsync(string name, CancellationToken ct);
    Task AddAsync(Category category, CancellationToken ct);
    void Delete(Category category, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
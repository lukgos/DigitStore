using Catalog.Module.Entities;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Repositories;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken ct);
    Task ChangeCategoryForProductsAsync(CategoryId oldCategoryId, CategoryId newCategoryId, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
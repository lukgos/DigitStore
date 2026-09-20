using Catalog.Module.Entities;
using Catalog.Module.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.DAL.Repositories;

public sealed class ProductRepository(CatalogDbContext dbContext) : IProductRepository
{
    public Task<Product?> GetByIdAsync(ProductId id, CancellationToken ct)
    {
        return dbContext.Products
            .Include(x => x.Images)
            .Include(x => x.PriceHistory)
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    
    public async Task AddAsync(Product product, CancellationToken ct)
    {
        await dbContext.Products.AddAsync(product, ct);
    }
    
    public Task UpdateAsync(Product product, CancellationToken ct)
    {
        dbContext.Products.Update(product);
        return Task.CompletedTask; 
    }
    
    public Task ChangeCategoryForProductsAsync(CategoryId oldCategoryId, CategoryId newCategoryId, CancellationToken ct)
    {
        return dbContext.Products
            .Where(p => p.CategoryId == oldCategoryId)
            .ExecuteUpdateAsync(s => s.SetProperty(p => p.CategoryId, newCategoryId), ct);
    }

    public Task SaveChangesAsync(CancellationToken ct)
    {
        return dbContext.SaveChangesAsync(ct);
    }
}
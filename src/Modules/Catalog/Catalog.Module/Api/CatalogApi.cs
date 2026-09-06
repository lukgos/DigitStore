using Catalog.Contracts.Services;
using Catalog.Module.DAL;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Api;

public sealed class CatalogApi(CatalogDbContext dbContext) : ICatalogApi
{
    public async Task<ProductDetailsDto?> GetProductAsync(Guid productId, CancellationToken ct)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == new ProductId(productId), ct);

        if (product is null)
        {
            return null;
        }

        return new ProductDetailsDto(product.Id.Value, product.Price.Value);
    }
}
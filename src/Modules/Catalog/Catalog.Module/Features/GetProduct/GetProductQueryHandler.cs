using Catalog.Module.DAL;
using Catalog.Module.DTOs;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Features.GetProduct;

public sealed class GetProductQueryHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> HandleAsync(GetProductQuery query, CancellationToken ct)
    {
        var productId = new ProductId(query.Id);

        var product = await dbContext.Products
            .AsNoTracking()
            .Where(x => x.Id == productId)
            .Select(x => new ProductDto(
                x.Id.Value, 
                x.Name, 
                x.Description, 
                x.Price.Value, 
                x.CategoryId != null ? x.CategoryId.Value : null, 
                x.Tags, 
                x.Images.Select(i => new ProductImageDto(i.Id.Value, i.Url, i.AltText, i.IsPrimary)), 
                x.Attributes,
                x.Version
            ))
            .SingleOrDefaultAsync(ct);

        return product;
    }
}
using Catalog.Module.ValueObjects;
using Shared.Abstractions.Common;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Entities;

public sealed class ProductPriceHistory : Entity<ProductPriceHistoryId>
{
    public ProductId ProductId { get; private set; }
    public Money Price { get; private set; }
    public DateTimeOffset ValidFrom { get; private set; }
    
    public Product Product { get; private set; } = null!;

    private ProductPriceHistory() 
    {
    }

    private ProductPriceHistory(ProductId productId, Money price, DateTimeOffset validFrom)
    {
        ProductId = productId;
        Price = price;
        ValidFrom = validFrom;
    }

    public static ProductPriceHistory Create(ProductPriceHistoryId id, ProductId productId, Money price, DateTimeOffset validFrom)
    {
        return new ProductPriceHistory(productId, price, validFrom)
        {
            Id = id
        };
    }
}
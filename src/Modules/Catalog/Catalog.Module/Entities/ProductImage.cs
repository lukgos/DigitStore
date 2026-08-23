using Catalog.Module.ValueObjects;
using Shared.Abstractions.Common;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Entities;

public sealed class ProductImage : Entity<ProductImageId>
{
    public ProductId ProductId { get; private set; }
    public Product Product { get; private set; } = null!;
    public string Url { get; private set; }
    public string AltText { get; private set; }
    public bool IsPrimary { get; private set; }
    

    private ProductImage()
    {
    }

    private ProductImage(ProductId productId, string url, string altText, bool isPrimary)
    {
        ProductId = productId;
        Url = url;
        AltText = altText;
        IsPrimary = isPrimary;
    }

    public static ProductImage Create(ProductImageId id, ProductId productId, string url, string altText, bool isPrimary)
    {
        return new ProductImage(productId, url, altText, isPrimary)
        {
            Id = id
        };
    }

    public void SetAsPrimary()
    {
        IsPrimary = true;
    }
    
    public void RemovePrimary()
    {
        IsPrimary = false;
    }
}
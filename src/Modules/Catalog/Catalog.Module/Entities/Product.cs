using Catalog.Module.ValueObjects;
using Shared.Abstractions.Common;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Entities;

public sealed class Product : AuditableEntity<ProductId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    
    public CategoryId? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    private readonly List<ProductImage> _images = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    
    
    private readonly List<string> _tags = new();
    public IReadOnlyCollection<string> Tags => _tags.AsReadOnly();
    

    private readonly Dictionary<string, string> _attributes = new();
    public IReadOnlyDictionary<string, string> Attributes => _attributes.AsReadOnly();
    
    private readonly List<ProductPriceHistory> _priceHistory = new();
    public IReadOnlyCollection<ProductPriceHistory> PriceHistory => _priceHistory.AsReadOnly();
    

    private Product()
    {
    }
    
    private Product(string name, string description, Money price, CategoryId? categoryId, IEnumerable<string> tags, Dictionary<string, string> attributes)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        
        _tags.AddRange(tags);
        _attributes = attributes;
    }

    public static Product Create(ProductId id, string name, string description, Money price, CategoryId? categoryId, IEnumerable<string> tags, Dictionary<string, string> attributes)
    {
        return new Product(name, description, price, categoryId, tags, attributes)
        {
            Id = id
        };
    }

    public void UpdateDetails(string name, string description, Money price) 
    {
        Name = name;
        Description = description;
        Price = price;
    }

    public void SetCategory(CategoryId? categoryId)
    {
        CategoryId = categoryId;
    }

    public void UpdateAttributes(Dictionary<string, string> attributes)
    {
        _attributes.Clear();

        foreach (var attribute in attributes)
        {
            _attributes.Add(attribute.Key, attribute.Value);
        }
    }

    public void UpdateTags(IEnumerable<string> tags)
    {
        _tags.Clear();
        _tags.AddRange(tags);
    }

    public void UpdateImages(IEnumerable<ProductImage> images)
    {
        _images.Clear();
        _images.AddRange(images);
    }
    
    public void AddImage(ProductImage image)
    {
        if (!_images.Any())
        {
            image.SetAsPrimary();
        }
        _images.Add(image);
    }
    
    public void ChangePrice(Money newPrice, DateTimeOffset changeDate)
    {
        if (Price.Value == newPrice.Value) 
        {
            return;
        }

        var historyEntry = ProductPriceHistory.Create(new ProductPriceHistoryId(Guid.NewGuid()), Id, Price, changeDate);
        _priceHistory.Add(historyEntry);

        Price = newPrice;
    }
}
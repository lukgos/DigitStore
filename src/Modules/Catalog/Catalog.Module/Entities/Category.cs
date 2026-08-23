using Shared.Abstractions.Common;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Entities;

public sealed class Category : AuditableEntity<CategoryId>
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    
    private readonly List<Product> _products = new();
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private Category()
    {
    }

    private Category(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public static Category Create(CategoryId id, string name, string description)
    {
        return new Category(name, description)
        {
            Id = id
        };
    }
    
    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
namespace Search.Module.Documents;

public class ProductDocument
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public decimal Price { get; init; }
    public Guid? CategoryId { get; init; }
    public string? CategoryName { get; init; }
    public IEnumerable<string> Tags { get; init; } = [];
    public Dictionary<string, string> Attributes { get; init; } = new();
    public string? PrimaryImageUrl { get; init; }
}
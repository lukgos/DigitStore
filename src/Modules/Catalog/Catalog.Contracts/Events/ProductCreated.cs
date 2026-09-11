namespace Catalog.Contracts.Events;

public record ProductCreated( 
    Guid Id, 
    string Name, 
    string Description, 
    decimal Price, 
    Guid? CategoryId, 
    string? CategoryName, 
    IEnumerable<string> Tags, 
    IReadOnlyDictionary<string, string> Attributes, 
    string? PrimaryImageUrl
);
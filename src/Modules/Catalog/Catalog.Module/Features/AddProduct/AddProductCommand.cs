using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.AddProduct;

public record AddProductCommand(Guid Id, string Name, string Description, decimal Price, Guid? CategoryId, IEnumerable<string> Tags, 
    IEnumerable<ProductImageCommand> Images, Dictionary<string, string> Attributes ) : ICommand;

public record ProductImageCommand(string Url, string AltText, bool IsPrimary);

public record AddProductRequest(string Name, string Description, decimal Price, Guid? CategoryId, IEnumerable<string>? Tags, IEnumerable<ProductImageCommand>? Images, Dictionary<string, string>? Attributes);
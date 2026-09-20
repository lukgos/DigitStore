using Catalog.Module.Features.AddProduct;
using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.UpdateProduct;

public record UpdateProductCommand(Guid Id, string Name, string Description, decimal Price, Guid? CategoryId, 
    IEnumerable<string> Tags, IEnumerable<ProductImageCommand> Images, Dictionary<string, string> Attributes) : ICommand;

public record UpdateProductRequest(string Name, string Description, decimal Price, Guid? CategoryId, IEnumerable<string>? Tags, 
    IEnumerable<ProductImageCommand>? Images, Dictionary<string, string>? Attributes);
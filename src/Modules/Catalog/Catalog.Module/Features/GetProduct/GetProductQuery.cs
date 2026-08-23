using Catalog.Module.DTOs;
using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.GetProduct;

public record GetProductQuery(Guid Id) : IQuery<ProductDto?>;


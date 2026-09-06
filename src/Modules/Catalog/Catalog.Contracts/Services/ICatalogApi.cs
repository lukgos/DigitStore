namespace Catalog.Contracts.Services;

public record ProductDetailsDto(Guid Id, decimal Price);

public interface ICatalogApi
{
    Task<ProductDetailsDto?> GetProductAsync(Guid productId, CancellationToken ct);
}
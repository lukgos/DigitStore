using Catalog.Module.Entities;
using Catalog.Module.Repositories;
using Catalog.Module.ValueObjects;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Features.AddProduct;

public sealed class AddProductCommandHandler(IProductRepository productRepository) : ICommandHandler<AddProductCommand>
{
    public async Task HandleAsync(AddProductCommand command, CancellationToken ct)
    {
        var productId = new ProductId(command.Id);
        var price = new Money(command.Price);
        var categoryId = command.CategoryId.HasValue ? new CategoryId(command.CategoryId.Value) : null;

        var product = Product.Create(
            productId,
            command.Name,
            command.Description,
            price,
            categoryId,
            command.Tags,
            command.Attributes
        );

        foreach (var imgCommand in command.Images)
        {
            var image = ProductImage.Create(
                new ProductImageId(Guid.NewGuid()),
                productId,
                imgCommand.Url,
                imgCommand.AltText,
                imgCommand.IsPrimary
            );
                
            product.AddImage(image);
        }

        await productRepository.AddAsync(product, ct);
        await productRepository.SaveChangesAsync(ct);
    }
}
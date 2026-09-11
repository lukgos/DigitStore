using Catalog.Contracts.Events;
using Catalog.Module.Entities;
using Catalog.Module.Exceptions;
using Catalog.Module.Repositories;
using Catalog.Module.ValueObjects;
using MassTransit;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Features.AddProduct;

public sealed class AddProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, IPublishEndpoint publishEndpoint) : ICommandHandler<AddProductCommand>
{
    public async Task HandleAsync(AddProductCommand command, CancellationToken ct)
    {
        var categoryId = command.CategoryId.HasValue ? new CategoryId(command.CategoryId.Value) : null;

        string? categoryName = null;
        
        if (categoryId is not null)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId, ct);
            if (category is null)
            {
                throw new CategoryNotFoundException(categoryId.Value); 
            }
            categoryName = category.Name;
        }
        
        var productId = new ProductId(command.Id);
        var price = new Money(command.Price);

        var product = Product.Create(productId, command.Name, command.Description, price, categoryId, command.Tags, command.Attributes);

        foreach (var imgCommand in command.Images)
        {
            var image = ProductImage.Create(new ProductImageId(Guid.NewGuid()), productId, imgCommand.Url, imgCommand.AltText, imgCommand.IsPrimary);
                
            product.AddImage(image);
        }

        await productRepository.AddAsync(product, ct);
        await productRepository.SaveChangesAsync(ct);
        
        var primaryImageUrl = product.Images.FirstOrDefault(x => x.IsPrimary)?.Url;
        var productCreated = new ProductCreated(
            product.Id.Value,
            product.Name,
            product.Description,
            product.Price.Value,
            product.CategoryId?.Value,
            categoryName,
            product.Tags,
            product.Attributes,
            primaryImageUrl
        );
        
        await publishEndpoint.Publish(productCreated, ct);
    }
}
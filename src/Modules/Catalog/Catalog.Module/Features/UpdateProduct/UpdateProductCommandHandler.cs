using Catalog.Contracts.Events;
using Catalog.Module.Entities;
using Catalog.Module.Exceptions;
using Catalog.Module.Repositories;
using Catalog.Module.ValueObjects;
using MassTransit;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Features.UpdateProduct;

public sealed class UpdateProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository, 
    IPublishEndpoint publishEndpoint) : ICommandHandler<UpdateProductCommand>
{
    public async Task HandleAsync(UpdateProductCommand command, CancellationToken ct)
    {
        var productId = new ProductId(command.Id);
        var product = await productRepository.GetByIdAsync(productId, ct);
        
        if (product is null)
        {
            throw new ProductNotFoundException(command.Id);
        }

        string? categoryName = null;
        var categoryId = command.CategoryId.HasValue ? new CategoryId(command.CategoryId.Value) : null;

        if (categoryId is not null && product.CategoryId != categoryId)
        {
            var category = await categoryRepository.GetByIdAsync(categoryId, ct);
            if (category is null)
            {
                throw new CategoryNotFoundException(categoryId.Value); 
            }
            categoryName = category.Name;
        }

        var newPrice = new Money(command.Price);
        product.ChangePrice(newPrice, DateTimeOffset.UtcNow);

        product.UpdateDetails(command.Name, command.Description, product.Price);

        product.SetCategory(categoryId);
        product.UpdateTags(command.Tags);
        product.UpdateAttributes(command.Attributes);

        var updatedImages = command.Images.Select(img => 
            ProductImage.Create(new ProductImageId(Guid.NewGuid()), productId, img.Url, img.AltText, img.IsPrimary)).ToList();
                
        product.UpdateImages(updatedImages);

        await productRepository.UpdateAsync(product, ct);
        await productRepository.SaveChangesAsync(ct);
        
        var primaryImageUrl = product.Images.FirstOrDefault(x => x.IsPrimary)?.Url;
        
        var productUpdated = new ProductUpdated(product.Id.Value, product.Name, product.Description, product.Price.Value,
            product.CategoryId?.Value, categoryName, product.Tags, product.Attributes, primaryImageUrl);
        
        await publishEndpoint.Publish(productUpdated, ct);
    }
}
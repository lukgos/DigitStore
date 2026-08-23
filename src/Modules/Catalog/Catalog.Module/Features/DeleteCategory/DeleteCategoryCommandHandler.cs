using Catalog.Module.Entities;
using Catalog.Module.Exceptions;
using Catalog.Module.Repositories;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Features.DeleteCategory;

public sealed class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IProductRepository productRepository) : ICommandHandler<DeleteCategoryCommand>
{
    public async Task HandleAsync(DeleteCategoryCommand command, CancellationToken ct)
    {
        var categoryId = new CategoryId(command.Id);
        
        var category = await categoryRepository.GetByIdAsync(categoryId, ct);

        if (category is null)
        {
            throw new CategoryNotFoundException(command.Id);
        }

        if (category.Name == "Other")
        {
            throw new CannotDeleteDefaultCategoryException();
        }
        
        var otherCategory = await categoryRepository.GetByNameAsync("Other", ct);

        if (otherCategory is null)
        {
            otherCategory = Category.Create(new CategoryId(Guid.NewGuid()), "Other", "Default category");
            await categoryRepository.AddAsync(otherCategory, ct);
            await categoryRepository.SaveChangesAsync(ct); 
        }

        await productRepository.ChangeCategoryForProductsAsync(categoryId, otherCategory.Id, ct);

        categoryRepository.Delete(category, ct);
        await categoryRepository.SaveChangesAsync(ct);
    }
}
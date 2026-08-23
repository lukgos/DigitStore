using Catalog.Module.Entities;
using Catalog.Module.Exceptions;
using Catalog.Module.Repositories;
using Shared.Abstractions.CQRS;
using Shared.Abstractions.ValueObjects;

namespace Catalog.Module.Features.AddCategory;

public sealed class AddCategoryCommandHandler(ICategoryRepository categoryRepository) : ICommandHandler<AddCategoryCommand>
{
    public async Task HandleAsync(AddCategoryCommand command, CancellationToken ct)
    {
        var exists = await categoryRepository.ExistsByNameAsync(command.Name, ct);
        if (exists)
        {
            throw new CategoryAlreadyExistsException(command.Name);
        }

        var categoryId = new CategoryId(command.Id);

        var category = Category.Create(categoryId, command.Name, command.Description);

        await categoryRepository.AddAsync(category, ct);
        await categoryRepository.SaveChangesAsync(ct);
    }
}
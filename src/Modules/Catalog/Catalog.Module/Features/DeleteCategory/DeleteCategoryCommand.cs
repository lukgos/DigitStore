using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.DeleteCategory;

public record DeleteCategoryCommand(Guid Id) : ICommand;


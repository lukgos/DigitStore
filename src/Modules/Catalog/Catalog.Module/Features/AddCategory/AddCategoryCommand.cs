using Shared.Abstractions.CQRS;

namespace Catalog.Module.Features.AddCategory;

public record AddCategoryCommand(Guid Id, string Name, string Description) : ICommand;


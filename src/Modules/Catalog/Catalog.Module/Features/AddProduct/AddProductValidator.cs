using FluentValidation;

namespace Catalog.Module.Features.AddProduct;

public class AddProductValidator : AbstractValidator<AddProductCommand>
{
    public AddProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
        RuleForEach(x => x.Images).ChildRules(images =>
        {
            images.RuleFor(i => i.Url).NotEmpty().MaximumLength(2000);
            images.RuleFor(i => i.AltText).MaximumLength(500);
        });
    }
}


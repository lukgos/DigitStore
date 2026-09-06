using FluentValidation;

namespace Order.Module.Features.CreateOrderFromProduct;

public class CreateOrderFromProductValidator : AbstractValidator<CreateOrderFromProductCommand>
{
    public CreateOrderFromProductValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();

        RuleFor(x => x.CustomerId).NotEmpty();

        RuleFor(x => x.ProductId).NotEmpty();

        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
using FluentValidation;

namespace Order.Module.Features.GetOrderById;

public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
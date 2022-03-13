namespace OrderService.Application.Features.Orders;

public class PayOrderCommandValidator : AbstractValidator<PayOrderCommand>
{
    public PayOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEqual(Guid.Empty);
    }
}
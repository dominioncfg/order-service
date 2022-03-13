namespace OrderService.Application.Features.Orders;

public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEqual(Guid.Empty);
    }
}
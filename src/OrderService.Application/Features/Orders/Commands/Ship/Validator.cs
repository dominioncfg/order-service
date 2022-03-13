namespace OrderService.Application.Features.Orders;

public class ShipOrderCommandValidator : AbstractValidator<ShipOrderCommand>
{
    public ShipOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEqual(Guid.Empty);
    }
}
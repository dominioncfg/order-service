namespace OrderService.Application.Features.Orders;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEqual(Guid.Empty);

        RuleFor(v => v.BuyerId)
           .NotEqual(Guid.Empty);

        RuleFor(x => x.Address).NotNull().SetValidator(new CreateOrderCommandAddressValidator());

        RuleFor(v => v.Items)
           .NotNull()
           .NotEmpty()
           .WithMessage("Order without any items.");

        RuleFor(v => v.Items)
           .Must(x => AllQuantitiesAreValid(x.Select(x => x.Quantity)))
           .NotNull()
           .NotEmpty()
           .WithMessage("Order with negative quantities.");

        RuleFor(v => v.Items)
         .Must(x => AllValidProductSku(x.Select(x => x.Sku)))
         .NotNull()
         .NotEmpty()
         .WithMessage("Order with negative quantities.");

        RuleFor(v => v.Items)
        .Must(x => AllPricesAreValid(x.Select(x => x.UnitPrice)))
        .NotNull()
        .NotEmpty()
        .WithMessage("Order with negative quantities.");
    }

    private static bool AllValidProductSku(IEnumerable<string> skus) => skus.All(x => !string.IsNullOrEmpty(x));

    private static bool AllQuantitiesAreValid(IEnumerable<int> quantities) => quantities.All(x => x > 0);

    private static bool AllPricesAreValid(IEnumerable<decimal> prices) => prices.All(x => x > 0);
}


class CreateOrderCommandAddressValidator : AbstractValidator<CreateOrderCommandAddress>
{
    public CreateOrderCommandAddressValidator()
    {
        RuleFor(x => x).NotNull();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.Street).NotEmpty();
        RuleFor(x => x.Number).NotEmpty();
    }
}
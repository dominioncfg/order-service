using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderService.Application.Features.Orders
{
    public class RegisterCarAdCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public RegisterCarAdCommandValidator()
        {
            RuleFor(v => v.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid Id.");

            RuleFor(v => v.Items)
               .NotNull()
               .NotEmpty()
               .WithMessage("Order without any items.");

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
        }

        private static bool AllValidProductSku(IEnumerable<string> skus) => skus.All(x => !string.IsNullOrEmpty(x));

        private static bool AllQuantitiesAreValid(IEnumerable<decimal> quantities) => quantities.All(x => x > 0);
    }
}

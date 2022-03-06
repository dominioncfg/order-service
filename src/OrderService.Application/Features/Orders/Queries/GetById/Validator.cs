using FluentValidation;
using System;

namespace OrderService.Application.Features.Orders;

public class GetOrderByIdQueryValidator : AbstractValidator<GetOrderByIdQuery>
{
    public GetOrderByIdQueryValidator()
    {
        RuleFor(v => v.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Invalid Id.");
    }
}

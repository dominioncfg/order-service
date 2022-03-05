using MediatR;
using OrderService.Application.Common.Exceptions;
using OrderService.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Orders;

public record CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;

    public CreateOrderCommandHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        await CheckOrderDontExist(request.Id, cancellationToken);

        var orderItems = new List<OrderItem>();
        foreach (var item in request.Items)
            orderItems.Add(new OrderItem(item.Sku, item.Quantity));

        var order = new Order(request.Id, orderItems.ToArray());


        await _ordersRepository.AddAsync(order, cancellationToken);
        return Unit.Value;
    }

    private async Task CheckOrderDontExist(Guid orderId, CancellationToken cancellationToken)
    {
        var existingOrder = await _ordersRepository.GetByIdOrDefaultAsync(orderId, cancellationToken);

        if (existingOrder is not null)
            throw new BadRequestApplicatonException("Order Already exist.");
    }
}

using MediatR;
using OrderService.Application.Common.Exceptions;
using OrderService.Domain.Orders;
using System;
using System.Linq;
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

        var orderItems = request.Items
            .Select(x => new CreateOrderItemArgs 
            { 
                Sku = x.Sku, 
                UnitPrice = -1,
                Quantity = (int)x.Quantity,
            })
            .ToArray();

        var createArgs = new CreateOrderArgs()
        {
            Id = request.Id,
            Items = orderItems,
        };
        var order = OrderFactory.Create(createArgs);


        await _ordersRepository.AddAsync(order, cancellationToken);
        return Unit.Value;
    }

    private async Task CheckOrderDontExist(Guid orderId, CancellationToken cancellationToken)
    {
        var existingOrder = await _ordersRepository.GetByIdOrDefaultAsync(orderId, cancellationToken);

        if (existingOrder is not null)
            throw new BadRequestApplicationException("Order Already exist.");
    }
}

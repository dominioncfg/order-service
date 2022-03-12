namespace OrderService.Application.Features.Orders;

public record CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IClockService _clockService;

    public CreateOrderCommandHandler(IOrdersRepository ordersRepository, IClockService clockService)
    {
        _ordersRepository = ordersRepository;
        _clockService = clockService;
    }

    public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var createdAt = _clockService.UtcNow;
        await CheckOrderDontExist(request.Id, cancellationToken);

        var orderItems = request.Items
            .Select(x => new CreateOrderItemArgs
            {
                Sku = x.Sku,
                UnitPrice = x.UnitPrice,
                Quantity = x.Quantity,
            })
            .ToArray();

        var createArgs = new CreateOrderArgs()
        {
            Id = request.Id,
            BuyerId = request.BuyerId,
            CreationDateTimeUtc = createdAt,
            Address = new()
            {
                Country = request.Address.Country,
                City = request.Address.City,
                Street = request.Address.Street,
                Number = request.Address.Number,
            },
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

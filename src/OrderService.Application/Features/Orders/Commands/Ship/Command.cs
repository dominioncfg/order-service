namespace OrderService.Application.Features.Orders;

public record ShipOrderCommand : IRequest
{
    public Guid Id { get; init; }
}
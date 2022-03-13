namespace OrderService.Application.Features.Orders;

public record CancelOrderCommand : IRequest
{
    public Guid Id { get; init; }
}
namespace OrderService.Application.Features.Orders;

public record PayOrderCommand : IRequest
{
    public Guid Id { get; init; }
}
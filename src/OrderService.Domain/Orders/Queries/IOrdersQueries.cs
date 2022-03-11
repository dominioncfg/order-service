namespace OrderService.Domain.Orders;

public interface IOrdersQueries
{
    Task<GetOrderByIdResponse?> GetOrderIdOrDefaultAsync(Guid orderId, CancellationToken cancellationToken);
    Task<GetAllOrdersResponse> GetAllOrdersAsync(CancellationToken cancellationToken);
}

using Microsoft.AspNetCore.Mvc;

namespace OrderService.Consumer.Features.Order;

[Route("api/orders/")]
[ApiController]
public class OrderProjectionController : ControllerBase
{
    private readonly IOrderProjectionRepository _repository;
    public OrderProjectionController(IOrderProjectionRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<Order>> GetAll(CancellationToken cancellationToken) => await _repository.GetAllAsync(cancellationToken);
}

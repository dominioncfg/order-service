using OrderService.Application.Features.Orders;

namespace OrderService.Api.Features.Orders;

[Route("api/orders")]
public class OrdersController : ApiControllerBase
{
    [HttpGet()]
    public async Task<GetAllOrdersQueryApiResponse> GetByIdAsync(CancellationToken cancellationToken)
    {
        var query = new GetAllOrdersQuery();
        var queryResponse = await Mediator.Send(query, cancellationToken);
        var response = Mapper.Map<GetAllOrdersQueryApiResponse>(queryResponse);
        return response;
    }

    [HttpGet("{id}")]
    public async Task<GetOrderByIdQueryApiResponse> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery() { Id = id };
        var queryResponse = await Mediator.Send(query, cancellationToken);
        var response = Mapper.Map<GetOrderByIdQueryApiResponse>(queryResponse);
        return response;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CreateOrderApiRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateOrderCommand>(request);
        await Mediator.Send(command, cancellationToken);
        return CreatedAtAction("GetById", new { });
    }
}
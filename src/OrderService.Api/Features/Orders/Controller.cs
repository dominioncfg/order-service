using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Common;
using OrderService.Application.Features.Orders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Api.Features.Orders;


[Route("api/orders")]
public class OrdersController : ApiControllerBase
{

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] CreateOrderApiRequest request, CancellationToken cancellationToken)
    {
        var command = Mapper.Map<CreateOrderCommand>(request);
        await Mediator.Send(command, cancellationToken);
        return CreatedAtAction("GetById", new { });
    }

    [HttpGet("{id}")]
    public async Task<GetOrderByIdQueryApiResponse> GetByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var query = new GetOrderByIdQuery() { Id = id };
        var queryResponse = await Mediator.Send(query, cancellationToken);
        var response = Mapper.Map<GetOrderByIdQueryApiResponse>(queryResponse);
        return response;
    }
}
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.Common;
using OrderService.Application.Features.Orders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Api.Features.Orders
{
    [Route("api/orders")]
    public class OrdersController : ApiControllerBase
    {

        [HttpPost]
        public async Task<ActionResult> CreateAsync([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var command = Mapper.Map<CreateOrderCommand>(request);
            await Mediator.Send(command, cancellationToken);
            return Created(GetOrderByIdUrl(command.Id), new object());
        }

        private static string GetOrderByIdUrl(Guid id)
        {
            //Just an example since we don't need this for the exercise
            return $"api/orders/{id}";
        }
    }
}

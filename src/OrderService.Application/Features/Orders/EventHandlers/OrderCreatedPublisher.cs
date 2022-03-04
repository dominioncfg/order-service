using MassTransit;
using MediatR;
using OrderService.Contracts;
using OrderService.Domain.Events;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Features.Orders
{
    public class OrderCreatedPublisher : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderCreatedPublisher(IPublishEndpoint publishEndpoint)
        {
            this._publishEndpoint = publishEndpoint;
        }

        public async Task Handle(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            var mappedItems = domainEvent.Items.Select(x => new OrderCreatedOrderItemDto(x.Sku, x.Quantity)).ToArray();
            var mappedEvent = new OrderCreatedIntegrationEvent(domainEvent.OrderId, mappedItems);
            await _publishEndpoint.Publish(mappedEvent, cancellationToken);
        }
    }
}

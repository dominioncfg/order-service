using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure;

namespace OrderService.Api.FunctionalTests.Shared;

public static class OrdersExtensions
{
    public static async Task AssumeOrderInRepository(this TestServerFixture fixture, Order model)
    {
        await fixture.ExecuteScopeAsync(async services =>
        {
            var searchRepo = services.GetRequiredService<IOrdersRepository>();
            await searchRepo.AddAsync(model, default);
        });
    }

    public static async Task<List<Order>> GetAllOrdersInRepository(this TestServerFixture fixture)
    {
        var orders = new List<Order>();
        await fixture.ExecuteScopeAsync(async services =>
        {
            var ordersDbContext = services.GetRequiredService<OrdersDbContext>();

            orders = await ordersDbContext
                .Orders
                .Include(x => x.Items)
                .ToListAsync(default);
        });
        return orders;
    }

    public static async Task AssumeDefaultOrderInRepositoryAsync(this TestServerFixture fixture, Guid orderId)
    {
        Order existingOrder = GivenDefaultOrder(orderId);
        await fixture.AssumeOrderInRepository(existingOrder);
    }

    public static async Task AssumeDefaultPaidOrderInRepositoryAsync(this TestServerFixture fixture, Guid orderId)
    {
        Order existingOrder = GivenDefaultOrder(orderId);
        existingOrder.MarkAsPaid();
        existingOrder.ClearEvents();
        await fixture.AssumeOrderInRepository(existingOrder);
    }

    public static async Task AssumeDefaultShippedOrderInRepositoryAsync(this TestServerFixture fixture, Guid orderId)
    {
        Order existingOrder = GivenDefaultOrder(orderId);
        existingOrder.MarkAsPaid();
        existingOrder.MarkAsShipped();
        existingOrder.ClearEvents();
        await fixture.AssumeOrderInRepository(existingOrder);
    }

    public static async Task AssumeDefaultCancelledOrderInRepositoryAsync(this TestServerFixture fixture, Guid orderId)
    {
        Order existingOrder = GivenDefaultOrder(orderId);
        existingOrder.Cancel();
        existingOrder.ClearEvents();
        await fixture.AssumeOrderInRepository(existingOrder);
    }

    private static Order GivenDefaultOrder(Guid orderId)
    {
        return new OrderBuilder()
                    .WithId(orderId)
                    .WithBuyerId(Guid.NewGuid())
                    .WithCreationDateTime(new DateTime(2020, 02, 03).At(04, 50))
                    .WithAddress(address => address
                        .WithCountry("Spain")
                        .WithCity("Barcelone")
                        .WithStreet("Diagonal")
                        .WithNumber("20")
                    )
                    .WithItem(item => item
                        .WithSku("product-sku-02")
                        .WithUnitPrice(2)
                        .WithQuantity(4)
                    )
                    .Build();
    }
}

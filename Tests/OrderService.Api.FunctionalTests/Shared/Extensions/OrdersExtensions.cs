using Microsoft.Extensions.DependencyInjection;
using OrderService.Api.FunctionalTests.SeedWork;
using OrderService.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var searchRepo = services.GetRequiredService<IOrdersRepository>();
            orders = await searchRepo.GetAllAsync(default);
        });
        return orders;
    }
}

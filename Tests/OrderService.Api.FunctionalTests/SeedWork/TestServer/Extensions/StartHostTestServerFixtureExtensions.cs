using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure;

namespace OrderService.Api.FunctionalTests.SeedWork;

public static class StartHostTestServerFixtureExtensions
{
    public static async Task RecreatePostgresDbBeforeStartAsync(this TestServerFixture given)
    {
        await given.ExecuteScopeAsync(async services =>
        {
            var dbContext = services.GetRequiredService<OrdersDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
        });
    }

    public static async Task StartMassTransitTransportAsync(this TestServerFixture given)
    {
        var testHarness = given.Server.Services.GetRequiredService<InMemoryTestHarness>();
        testHarness.TestTimeout = TimeSpan.FromSeconds(3);
        testHarness.TestInactivityTimeout = TimeSpan.FromSeconds(3);
        await testHarness.Start();
    }
}

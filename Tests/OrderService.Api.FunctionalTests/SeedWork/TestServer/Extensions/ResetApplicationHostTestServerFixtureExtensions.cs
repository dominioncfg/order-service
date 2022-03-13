using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Infrastructure;

namespace OrderService.Api.FunctionalTests.SeedWork;

public static class ResetApplicationHostTestServerFixtureExtensions
{
    public static void OnTestInitResetApplicationServices(this TestServerFixture _)
    {
        MockClockService.ResetService();
    }

    public static async Task OnTestInitResetPublishedEventsAsync(this TestServerFixture given)
    {
        //TODO: Not the best way of integration test with Mass Transit
        await given.ExecuteScopeAsync(services =>
        {
            var testHarness = services.GetRequiredService<InMemoryTestHarness>();
            given.ResetBus();
            testHarness.Bus.ConnectPublishObserver(given.CurrentTestPublishedEvents);
            return Task.CompletedTask;
        });
    }

    public static async Task OnTestInitResetPostgresDbAsync(this TestServerFixture given)
    {
        await given!.ExecuteScopeAsync(async services =>
        {
            var dataStorage = services.GetRequiredService<OrdersDbContext>();
            await dataStorage.ClearDatabaseBeforeTestAsync();
        });
    }
}
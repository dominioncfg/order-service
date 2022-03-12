using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Api.FunctionalTests.SeedWork;

public static class TestServerFixtureExtensions
{
    public static async Task ExecuteScopeAsync(this TestServerFixture fixture, Func<IServiceProvider, Task> function)
    {
        var scopeFactory = fixture.Server.Services.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        await function(scope.ServiceProvider);
    }
}

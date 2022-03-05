using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

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

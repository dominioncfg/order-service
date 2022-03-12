using MassTransit.Testing;
using MassTransit.Testing.Observers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Infrastructure;

namespace OrderService.Api.FunctionalTests.SeedWork;

public sealed class TestServerFixture : IDisposable
{
    public TestServer Server { get; }
    private static TestServerFixture? FixtureInstance { get; set; }
    public static BusTestPublishObserver? CurrentTestPublishedEvents { get; private set; }

    public TestServerFixture()
    {
        var hostBuilder = new HostBuilder()
            .ConfigureAppConfiguration((context, builder) =>
            {
                builder
                   .SetBasePath(context.HostingEnvironment.ContentRootPath)
                   .AddJsonFile("appsettings.json");
            })
            .UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                services.AddMassTransitInMemoryTestHarness();
            })
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.UseStartup<Startup>();
                webHost.ConfigureTestServices(services =>
                {
                    services.SwapSingleton<IClockService, MockClockService>(MockClockService.Service);
                });
            });

        var host = hostBuilder.StartAsync().GetAwaiter().GetResult();
        this.Server = host.GetTestServer();
        FixtureInstance = this;
        StartMassTransitTransport();
        RecreateDatabaseOnInitAsync().GetAwaiter().GetResult();
    }

    private void StartMassTransitTransport()
    {
        var testHarness = Server.Services.GetRequiredService<InMemoryTestHarness>();
        testHarness.TestTimeout = TimeSpan.FromSeconds(3);
        testHarness.TestInactivityTimeout = TimeSpan.FromSeconds(3);
        testHarness.Start().GetAwaiter().GetResult();
    }

    public void Dispose()
    {
        Server.Dispose();
    }

    #region Recreate Databases On Start
    private static async Task RecreateDatabaseOnInitAsync()
    {
        await RecreatePostgresDbBeforeStart();
    }
    private static async Task RecreatePostgresDbBeforeStart()
    {
        if (FixtureInstance is null)
            throw new Exception("Test Setup Error");

        await FixtureInstance.ExecuteScopeAsync(async services =>
        {
            var dbContext = services.GetRequiredService<OrdersDbContext>();
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();
        });
    }
    #endregion

    #region Reset Application State Between Test
    public static void OnTestInitResetApplicationState()
    {
        OnTestInitResetApplicationServices();
        OnTestInitResetPostgresDb().GetAwaiter().GetResult();
        OnTestInitResetPublishedEvents().GetAwaiter().GetResult();
    }

    private static void OnTestInitResetApplicationServices()
    {
        MockClockService.ResetService();
    }

    private static async Task OnTestInitResetPublishedEvents()
    {
        //TODO: Not the best way of integration test with Mass Transit
        await FixtureInstance!.ExecuteScopeAsync(services =>
        {
            var testHarness = services.GetRequiredService<InMemoryTestHarness>();
            CurrentTestPublishedEvents = new BusTestPublishObserver(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), default);
            testHarness.Bus.ConnectPublishObserver(CurrentTestPublishedEvents);
            return Task.CompletedTask;
        });
    }

    private static async Task OnTestInitResetPostgresDb()
    {
        await FixtureInstance!.ExecuteScopeAsync(async services =>
        {
            var dataStorage = services.GetRequiredService<OrdersDbContext>();
            await dataStorage.ClearDatabaseBeforeTestAsync();
        });
    }
    #endregion
}

public static class ServiceCollectionExtension
{
    public static void SwapSingleton<TService, TImplementation>(this IServiceCollection services, TImplementation implementation)
        where TImplementation : class, TService
    {
        var serviceDescriptors = services.Where(x => x.ServiceType == typeof(TService) && x.Lifetime == ServiceLifetime.Singleton).ToList();

        if (!serviceDescriptors.Any())
            throw new ArgumentException($"There is no original implementation for {nameof(TService)}.", nameof(TService));

        foreach (var serviceDescriptor in serviceDescriptors)
        {
            services.Remove(serviceDescriptor);
        }
        services.AddSingleton(typeof(TService), implementation);
    }
}
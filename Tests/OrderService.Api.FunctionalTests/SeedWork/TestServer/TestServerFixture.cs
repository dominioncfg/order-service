using MassTransit.Testing;
using MassTransit.Testing.Observers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OrderService.Api.FunctionalTests.SeedWork;

public sealed class TestServerFixture : IDisposable
{
    public TestServer Server { get; }
    private static TestServerFixture? FixtureInstance { get; set; }
    public BusTestPublishObserver? CurrentTestPublishedEvents { get; private set; }

    public static void OnTestInitResetApplicationState()
    {
        FixtureInstance!.OnTestInitResetApplicationServices();
        FixtureInstance!.OnTestInitResetPostgresDbAsync().GetAwaiter().GetResult();
        FixtureInstance!.OnTestInitResetPublishedEventsAsync().GetAwaiter().GetResult();
    }

    public TestServerFixture()
    {
        IHostBuilder hostBuilder = ConfigureHost();
        var host = hostBuilder.StartAsync().GetAwaiter().GetResult();
        Server = host.GetTestServer();
        FixtureInstance = this;
        FixtureInstance!.StartMassTransitTransportAsync().GetAwaiter().GetResult();
        FixtureInstance!.RecreatePostgresDbBeforeStartAsync().GetAwaiter().GetResult();
    }
    
    public void ResetBus()
    {
        CurrentTestPublishedEvents = new BusTestPublishObserver(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2), default);
    }

    public void Dispose() => Server.Dispose();

    private static IHostBuilder ConfigureHost()
    {
        return new HostBuilder()
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
    }
}

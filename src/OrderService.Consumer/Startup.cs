using OrderService.Consumer.Configuration;
using OrderService.Consumer.Features.Order;

namespace OrderService.Consumer;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCustomSwagger()
            .AddSingleton(new List<Order>())
            .AddTransient<IOrderProjectionRepository, InMemoryOrderProjectionRepository>()
            .AddCustomMassTransit(_configuration)
            .AddControllers();
    }

    public void Configure(IApplicationBuilder app)
    {
        app
            .UseCustomSwagger()
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
    }
}

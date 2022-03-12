using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OrderService.Domain.Orders;
using OrderService.Infrastructure.DbQueries;
using OrderService.Infrastructure.Queries;
using OrderService.Infrastructure.Repositories;
using OrderService.Seedwork.Domain;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddOrderServiceInfraestructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext(configuration)
            .AddRepositories()
            .AddServices()
            .AddQueries(configuration);
    }

    public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app)
    {
        InitializeDatabase(app);
        return app;
    }

    private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrdersDb");
        IServiceCollection serviceCollection = services.AddDbContext<OrdersDbContext>(options => 
            options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention()
            );
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IOrdersRepository, OrdersRepository>();
        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IClockService, ClockService>();
        return services;
    }

    private static IServiceCollection AddQueries(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("OrdersDb");
        services.AddScoped<IOrdersDbQuery, OrdersDbQuery>(opts => new OrdersDbQuery(new NpgsqlConnection(connectionString))); 
        services.AddTransient<IOrdersQueries, OrdersQueries>();
        return services;
    }

    private static void InitializeDatabase(IApplicationBuilder app)
    {
        var serviceFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();

        using var serviceScope = serviceFactory.CreateScope();
        MigrateOrdersDatabaseAsync(serviceScope).GetAwaiter().GetResult();
    }

    private static async Task MigrateOrdersDatabaseAsync(IServiceScope serviceScope)
    {
        //Not recomended for production, is best to use CI-CD for this
        await serviceScope.ServiceProvider.GetRequiredService<OrdersDbContext>().Database.MigrateAsync();
    }
}

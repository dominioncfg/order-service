using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Domain;
using OrderService.Infrastructure.Repositories;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Configuration
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOrderServiceInfraestructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext(configuration)
                .AddRepositories();
        }

        public static IApplicationBuilder UseInfraestructure(this IApplicationBuilder app)
        {
            InitializeDatabase(app);
            return app;
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var something = configuration.GetConnectionString("OrdersDb");
            IServiceCollection serviceCollection = services.AddDbContext<OrdersDbContext>(options => options.UseNpgsql(something));
            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IOrdersRepository, OrdersRepository>();
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
            //Not recomended for production
            await serviceScope.ServiceProvider.GetRequiredService<OrdersDbContext>().Database.MigrateAsync();
        }
    }
}

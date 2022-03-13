using Microsoft.Extensions.DependencyInjection;

namespace OrderService.Api.FunctionalTests.SeedWork;

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
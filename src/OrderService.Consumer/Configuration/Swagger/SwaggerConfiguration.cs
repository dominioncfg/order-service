using Microsoft.OpenApi.Models;

namespace OrderService.Consumer.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "V1",
                Title = "Order Service Projection",
                Description = "Backend Api For Order",
            });
            c.DescribeAllParametersInCamelCase();
            c.CustomSchemaIds(t => t.Name);
        });

        return services;
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service API V1");
        });
        return app;
    }
}

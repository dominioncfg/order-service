using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Api.Configuration;
using OrderService.Infrastructure.Configuration;

namespace OrderService.Api;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        this._configuration = configuration;
        this._env = env;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddCustomFluentValidation()
            .AddCustomAutomapper()
            .ConfigureProblemDetails()
            .AddCustomMassTransit(_env, _configuration)
            .AddCustomMediatR()
            .AddOrderServiceInfraestructure(_configuration)
            .AddCustomSwagger()
            .AddMvc();
    }

    public void Configure(IApplicationBuilder app)
    {
        app
            .UseInfraestructure()
            .UseCustomProblemDetails()
            .UseRouting()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            })
            .UseCustomSwagger();
    }
}

using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Server.Configuration;
using Server.Discovery;

namespace Server.AutumnBoot;

/// <summary>
/// Entry point and bootstrapper for the AutumnBoot application.
/// </summary>
/// <remarks>
/// This class sets up the ASP.NET Core <see cref="WebApplication"/> by:
/// <list type="number">
///   <item>Initializing logging and displaying a startup banner.</item>
///   <item>Configuring controllers, JSON serialization, and Swagger/OpenAPI.</item>
///   <item>Registering application services via dependency injection (DI) using <see cref="ServiceDiscovery"/>.</item>
///   <item>Configuring HTTP clients and static configuration.</item>
///   <item>Applying middleware for error handling and Swagger UI.</item>
///   <item>Building and running the web server.</item>
/// </list>
/// </remarks>
public static class AutumnBootApplication
{
    /// <summary>
    /// Configures and runs the AutumnBoot web application.
    /// </summary>
    /// <param name="assembly">The assembly to scan for services and controllers.</param>
    /// <param name="args">Command-line arguments passed to the application.</param>
    /// <remarks>
    /// This method orchestrates the full application startup process:
    /// <list type="bullet">
    ///   <item>Creates a <see cref="WebApplicationBuilder"/> and sets up logging.</item>
    ///   <item>Loads and logs the ASCII banner via <see cref="Banner.Load(ILogger)"/>.</item>
    ///   <item>Configures controllers with JSON serialization using <c>Newtonsoft.Json</c>.</item>
    ///   <item>Registers Swagger/OpenAPI documentation.</item>
    ///   <item>Automatically registers discovered services (Singleton/Scoped/Transient).</item>
    ///   <item>Registers an <c>HttpClient</c> named <c>alfred</c> with a predefined base address.</item>
    ///   <item>Configures static resources and middleware (error handler, Swagger, controllers).</item>
    ///   <item>Builds and runs the web server.</item>
    /// </list>
    /// </remarks>
    public static void Run(Assembly assembly, string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger("AutumnBootApplication");
        
        Banner.Load(logger);

        builder.Services.AddControllers().AddNewtonsoftJson(NewtonsoftSerializer.Configure);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerConfiguration.OpenApi);
        
        // Dependency Injection references for Singleton/Scoped/Transient types
        LoadServices(logger, assembly, builder);
        
        builder.Services.AddHttpClient("alfred", client =>
        {
            client.BaseAddress = new Uri("https://backend.alfred.segreti.io");
        });
        

        WebApplication server = builder.Build();
        StaticConfiguration.Embedded(server);

        server.UseExceptionHandler("/error");
        server.UseSwagger(SwaggerConfiguration.Options);
        server.UseSwaggerUI(SwaggerConfiguration.UiOptions);
        server.MapControllers();

        // Start application
        server.Run();
    }

    private static void LoadServices(ILogger logger, Assembly assembly, WebApplicationBuilder builder)
    {
        foreach (ServiceDetails service in ServiceDiscovery.FindServices(assembly))
        {
            if (service is null)
            {
                continue;
            }
            
            logger.LogDebug("{Message}", $"Registering [{service.ServiceType}] as [{service.Interface}] -> [{service.Implementation}]");
            
            switch (service.ServiceType)
            {
                case ServiceType.Singleton:
                    builder.Services.AddSingleton(service.Interface, service.Implementation);
                    break;
                case ServiceType.Scoped:
                    builder.Services.AddScoped(service.Interface, service.Implementation);
                    break;
                case ServiceType.Transient:
                    builder.Services.AddTransient(service.Interface, service.Implementation);
                    break;
                default:
                    throw new ArgumentException("Unknown service type");
            }
        }
    }
}
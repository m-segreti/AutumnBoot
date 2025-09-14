using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Server.Configuration;
using Server.Services;

namespace Server.AutumnBoot;

public static class AutumnBootApplication
{
    public static void Run(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Banner.Load(builder);

        builder.Services.AddControllers().AddNewtonsoftJson(NewtonsoftSerializer.Configure);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerConfiguration.OpenApi);
        builder.Services.AddSingleton<IContractService, ContractService>();

        WebApplication server = builder.Build();
        StaticConfiguration.Embedded(server);

        server.UseExceptionHandler("/error");
        server.UseSwagger(SwaggerConfiguration.Options);
        server.UseSwaggerUI(SwaggerConfiguration.UiOptions);
        server.MapControllers();

        // Start application
        server.Run();
    }
}
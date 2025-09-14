using Server.Configuration;
using Server.Services;

namespace Server;

public static class CSharpServer
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers().AddNewtonsoftJson(NewtonsoftSerializer.Configure);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerConfiguration.OpenApi);
        builder.Services.AddSingleton<IContractService, ContractService>();

        WebApplication server = builder.Build();
        //app.UseExceptionHandler("/error");
        server.UseSwagger(SwaggerConfiguration.Options);
        server.UseSwaggerUI(SwaggerConfiguration.UiOptions);
        server.MapControllers();
        server.Run();
    }
}
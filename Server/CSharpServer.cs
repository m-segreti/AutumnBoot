using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Configuration;
using Server.Services;

namespace Server;

public class CSharpServer {
    public static void Main(string[] args) {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers().AddNewtonsoftJson(NewtonsoftSerializer.configure);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(SwaggerConfiguration.OpenApi);
        builder.Services.AddSingleton<IContractService, ContractService>();

        WebApplication app = builder.Build();
        app.UseExceptionHandler("/error");
        app.UseSwagger();
        app.UseSwaggerUI(SwaggerConfiguration.configure);
        app.MapControllers();
        app.Run();
    }
}

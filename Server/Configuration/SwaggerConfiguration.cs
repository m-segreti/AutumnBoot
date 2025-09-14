using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Server.Configuration;

public static class SwaggerConfiguration
{
    private const string Version = "v1";
    
    public static void UiOptions(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint("openapi.json", $"CSharp Server {Version}");
        options.RoutePrefix = $"api/{Version}/docs";
    }

    public static void Options(SwaggerOptions options)
    {
        options.RouteTemplate = "/api/{documentName}/docs/openapi.json";
    }

    public static void OpenApi(SwaggerGenOptions options)
    {
        options.SwaggerDoc(Version, OpenApiInfo());
    }

    private static OpenApiInfo OpenApiInfo()
    {
        return new OpenApiInfo
        {
            Title = $"CSharp Server API {Version}",
            Version = Version
        };
    }
}
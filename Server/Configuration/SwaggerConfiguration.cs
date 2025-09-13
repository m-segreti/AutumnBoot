using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Server.Configuration;

public static class SwaggerConfiguration {
    public static void configure(SwaggerUIOptions options) {
        options.RoutePrefix = "api/v1/docs";
    }

    public static void OpenApi(SwaggerGenOptions options) {
        options.SwaggerDoc("v1", openApiInfo());
    }

    private static OpenApiInfo openApiInfo() {
        return new OpenApiInfo {
            Title = "API v1",
            Version = "v1"
        };
    }
}

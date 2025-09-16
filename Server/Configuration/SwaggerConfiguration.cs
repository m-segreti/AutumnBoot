using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Server.Configuration;

/// <summary>
/// Centralized configuration for Swagger / OpenAPI documentation in the application.
/// </summary>
/// <remarks>
/// <para>
/// This class provides helper methods to configure:
/// <list type="bullet">
///   <item><see cref="SwaggerUIOptions"/> → The interactive Swagger UI for browsing APIs.</item>
///   <item><see cref="SwaggerOptions"/> → The JSON endpoint(s) that expose the OpenAPI specification.</item>
///   <item><see cref="SwaggerGenOptions"/> → The generation of the OpenAPI specification itself.</item>
/// </list>
/// </para>
/// <para>
/// By consolidating Swagger setup here, the configuration remains consistent and avoids duplication in <c>Program.cs</c> or <c>Startup</c>.
/// </para>
/// </remarks>
public static class SwaggerConfiguration
{
    private const string Version = "v1";

    /// <summary>
    /// Configures the Swagger UI endpoint and route prefix.
    /// </summary>
    /// <param name="options">The <see cref="SwaggerUIOptions"/> to configure.</param>
    /// <remarks>
    /// <para>
    /// This sets:
    /// <list type="bullet">
    ///   <item><c>SwaggerEndpoint("openapi.json", "CSharp Server v1")</c> → Points the UI to the generated OpenAPI spec.</item>
    ///   <item><c>RoutePrefix = "api/v1/docs"</c> → Serves Swagger UI at <c>/api/v1/docs</c>.</item>
    /// </list>
    /// </para>
    /// Example: navigate to <c>https://localhost:5001/api/v1/docs</c> to view the API documentation UI.
    /// </remarks>
    public static void UiOptions(SwaggerUIOptions options)
    {
        options.SwaggerEndpoint("openapi.json", $"AutumnBoot Server {Version}");
        options.RoutePrefix = $"api/{Version}/docs";
    }

    /// <summary>
    /// Configures the route template for serving Swagger/OpenAPI JSON documents.
    /// </summary>
    /// <param name="options">The <see cref="SwaggerOptions"/> to configure.</param>
    /// <remarks>
    /// <para>
    /// The route template is set to <c>/api/{documentName}/docs/openapi.json</c>.  
    /// For version <c>v1</c>, the spec will be served at:
    /// <c>/api/v1/docs/openapi.json</c>.
    /// </para>
    /// </remarks>
    public static void Options(SwaggerOptions options)
    {
        options.RouteTemplate = "/api/{documentName}/docs/openapi.json";
    }

    /// <summary>
    /// Configures OpenAPI generation for the application.
    /// </summary>
    /// <param name="options">The <see cref="SwaggerGenOptions"/> to configure.</param>
    /// <remarks>
    /// Adds a Swagger document named <c>v1</c> using the metadata from <see cref="OpenApiInfo"/>.
    /// </remarks>
    public static void OpenApi(SwaggerGenOptions options)
    {
        options.SwaggerDoc(Version, OpenApiInfo());
    }

    private static OpenApiInfo OpenApiInfo()
    {
        return new OpenApiInfo
        {
            Title = $"AutumnBoot Server API {Version}",
            Version = Version
        };
    }
}

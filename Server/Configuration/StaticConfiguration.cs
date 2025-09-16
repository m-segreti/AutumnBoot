using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Server.Configuration;

/// <summary>
/// Provides configuration helpers for serving static files in the ASP.NET Core application.
/// </summary>
/// <remarks>
/// <para>
/// This class supports two modes of serving static content:
/// <list type="bullet">
///   <item><see cref="Default(WebApplication)"/> → Serves static files from the default <c>wwwroot</c> folder on disk.</item>
///   <item><see cref="Embedded(WebApplication)"/> → Serves static files embedded as resources in the application assembly (useful for single-file deployments).</item>
/// </list>
/// </para>
/// <para>
/// Both approaches configure default documents (e.g., <c>index.html</c>) and static file serving.  
/// The <c>Embedded</c> mode uses <see cref="EmbeddedFileProvider"/> to locate files bundled within the assembly.
/// </para>
/// </remarks>
public static class StaticConfiguration
{
    /// <summary>
    /// Configures the application to serve static files from the default <c>wwwroot</c> directory.
    /// </summary>
    /// <param name="server">The <see cref="WebApplication"/> instance.</param>
    /// <remarks>
    /// <para>
    /// The configuration includes:
    /// <list type="bullet">
    ///   <item>Enabling <c>UseDefaultFiles()</c> → Serves <c>index.html</c> or other default files automatically.</item>
    ///   <item>Enabling <c>UseStaticFiles()</c> → Serves static content (CSS, JS, images) from <c>wwwroot</c>.</item>
    ///   <item>Mapping fallback route to <c>index.html</c> → Useful for Single Page Applications (SPAs) with client-side routing.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public static void Default(WebApplication server)
    {
        server.UseDefaultFiles();
        server.UseStaticFiles();
        server.MapFallbackToFile("/index.html");
    }

    /// <summary>
    /// Configures the application to serve static files embedded in the assembly.
    /// </summary>
    /// <param name="server">The <see cref="WebApplication"/> instance.</param>
    /// <remarks>
    /// <para>
    /// This is an alternative to <see cref="Default(WebApplication)"/> when you want to embed static assets
    /// (such as HTML, CSS, and JavaScript) directly into the application assembly instead of deploying them separately.  
    /// Useful for self-contained deployments or distributing a single executable.
    /// </para>
    /// <para>
    /// The method:
    /// <list type="number">
    ///   <item>Builds an <see cref="EmbeddedFileProvider"/> rooted in <c>Server.wwwroot</c>.</item>
    ///   <item>Creates custom <see cref="DefaultFilesOptions"/> that point to <c>index.html</c>.</item>
    ///   <item>Configures <see cref="StaticFileOptions"/> for serving embedded files.</item>
    ///   <item>Registers <c>UseDefaultFiles()</c> and <c>UseStaticFiles()</c> with these options.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public static void Embedded(WebApplication server)
    {
        EmbeddedFileProvider provider = BuildEmbeddedFileProvider();
        DefaultFilesOptions defaults = BuildDefaultFileOptions(provider);
        StaticFileOptions staticFiles = BuildStaticFileOptions(provider);

        server.UseDefaultFiles(defaults);
        server.UseStaticFiles(staticFiles);
    }

    private static EmbeddedFileProvider BuildEmbeddedFileProvider()
    {
        Assembly assembly = typeof(Server).Assembly;
        string baseNamespace = typeof(Server).Namespace + ".wwwroot";

        return new EmbeddedFileProvider(assembly, baseNamespace);
    }

    private static DefaultFilesOptions BuildDefaultFileOptions(EmbeddedFileProvider provider)
    {
        DefaultFilesOptions options = new DefaultFilesOptions
        {
            FileProvider = provider,
            RequestPath = string.Empty
        };

        options.DefaultFileNames.Clear();
        options.DefaultFileNames.Add("index.html");

        return options;
    }

    private static StaticFileOptions BuildStaticFileOptions(EmbeddedFileProvider provider)
    {
        return new StaticFileOptions
        {
            FileProvider = provider,
            RequestPath = string.Empty
        };
    }
}

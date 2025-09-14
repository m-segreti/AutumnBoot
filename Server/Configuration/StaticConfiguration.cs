using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;

namespace Server.Configuration;

public static class StaticConfiguration
{
    public static void Default(WebApplication server)
    {
        server.UseDefaultFiles();
        server.UseStaticFiles();
        server.MapFallbackToFile("/index.html");
    }

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
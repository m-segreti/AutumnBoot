using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server.Configuration;

public static class Banner
{
    public static void Load(IHostApplicationBuilder builder)
    {
        Assembly assembly = typeof(Banner).Assembly;
        string resourceName = assembly
            .GetManifestResourceNames()
            .FirstOrDefault(name => name.EndsWith("Resources.banner.txt", StringComparison.OrdinalIgnoreCase));

        if (resourceName is null)
        {
            return;
        }

        Stream stream = assembly.GetManifestResourceStream(resourceName)!;
        StreamReader reader = new StreamReader(stream);

        string content = reader.ReadToEnd();
        content = content.Replace("\\x1b", "\u001b");
        content = content.Replace("{datetime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));

        ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger("Banner");
        logger.LogInformation("{Banner}", content);
    }
}
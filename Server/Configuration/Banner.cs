using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Server.AutumnBoot;

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

        Assembly autumnBoot = typeof(AutumnBootApplication).Assembly;

        string content = reader.ReadToEnd();
        string applicationName = autumnBoot.GetName().Name;
        string versionInfo = autumnBoot.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "x.y.z";
        string[] versionDetails = versionInfo.Split("+");
        string applicationVersion = versionDetails[0];

        // If commit details are present
        if (versionDetails.Length > 1)
        {
            content = content.Replace("{commitId}", versionDetails[1]);
        }
        
        content = content.Replace("\\x1b", "\u001b");
        content = content.Replace("{datetime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        content = content.Replace("{applicationName}", applicationName);
        content = content.Replace("{applicationVersion}", applicationVersion);

        ILogger logger = builder.Services.BuildServiceProvider().GetRequiredService<ILoggerFactory>().CreateLogger("Banner");
        logger.LogInformation("{Banner}", content);
    }
}
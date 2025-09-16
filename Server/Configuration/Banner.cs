using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AnsiColors;
using Microsoft.Extensions.Logging;
using Server.AutumnBoot;

namespace Server.Configuration;

/// <summary>
/// Responsible for loading and displaying a formatted ASCII banner at application startup.
/// </summary>
/// <remarks>
/// The banner is retrieved from an embedded resource file named <c>Resources.banner.txt</c>.
/// Placeholders in the banner text (e.g., <c>{applicationName}</c>, <c>{applicationVersion}</c>, 
/// <c>{commitId}</c>, <c>{datetime}</c>) will be replaced dynamically at runtime.
/// 
/// This is typically called during the initialization phase of the application to log a 
/// startup message containing version and commit information.
/// </remarks>
public static class Banner
{
    /// <summary>
    /// Loads and displays the application banner from the embedded resource file.
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/> instance used to log the formatted banner.</param>
    /// <remarks>
    /// <para>
    /// The method performs the following steps:
    /// <list type="number">
    /// <item>Locates the embedded resource <c>Resources.banner.txt</c> in the current assembly.</item>
    /// <item>Reads the banner content as text.</item>
    /// <item>Retrieves the application assembly information from <see cref="AutumnBootApplication"/>.</item>
    /// <item>Extracts version and optional commit ID from the assembly's informational version attribute.</item>
    /// <item>Replaces placeholders in the banner text with runtime values:
    ///   <list type="bullet">
    ///   <item><c>{applicationName}</c> → The application’s assembly name.</item>
    ///   <item><c>{applicationVersion}</c> → The semantic version number.</item>
    ///   <item><c>{commitId}</c> → The commit hash if present in the version string.</item>
    ///   <item><c>{datetime}</c> → Current local date and time in <c>yyyy-MM-dd HH:mm:ss.fff</c> format.</item>
    ///   </list>
    /// </item>
    /// <item>Formats ANSI color codes using <see cref="Ansi.Format(string)"/>.</item>
    /// <item>Logs the formatted banner at <c>Information</c> level.</item>
    /// </list>
    /// </para>
    /// </remarks>
    public static void Load(ILogger logger)
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
        
        content = Ansi.Format(content);
        content = content.Replace("{datetime}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        content = content.Replace("{applicationName}", applicationName);
        content = content.Replace("{applicationVersion}", applicationVersion);

        logger.LogInformation("{Banner}", content);
    }
}
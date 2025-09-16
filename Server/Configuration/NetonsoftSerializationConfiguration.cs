using Microsoft.AspNetCore.Mvc;

namespace Server.Configuration;

/// <summary>
/// Provides centralized configuration for Newtonsoft.Json serialization in ASP.NET Core.
/// </summary>
/// <remarks>
/// <para>
/// This static helper class encapsulates configuration for <c>MvcNewtonsoftJsonOptions</c>, 
/// allowing you to apply consistent JSON serialization settings across the application.
/// </para>
/// <para>
/// By default, ASP.NET Core uses <c>System.Text.Json</c>. Adding
/// <c>.AddNewtonsoftJson(NewtonsoftSerializer.Configure)</c> during service registration
/// opts into Newtonsoft.Json, which may be useful when:
/// <list type="bullet">
///   <item>Working with advanced JSON scenarios (e.g., polymorphic serialization).</item>
///   <item>Using attributes only supported by Newtonsoft.Json.</item>
///   <item>Maintaining compatibility with legacy JSON formats.</item>
/// </list>
/// </para>
/// </remarks>
public static class NewtonsoftSerializer
{
    /// <summary>
    /// Configures the global <see cref="MvcNewtonsoftJsonOptions"/> used by ASP.NET Core controllers.
    /// </summary>
    /// <param name="options">The JSON options to configure.</param>
    /// <remarks>
    /// <para>
    /// The current implementation applies the following setting:
    /// <list type="bullet">
    ///   <item><see cref="Newtonsoft.Json.Formatting.Indented"/> → 
    ///   Ensures JSON output is human-readable with indentation.</item>
    /// </list>
    /// </para>
    /// <para>
    /// Example usage during service configuration:
    /// <code>
    /// builder.Services.AddControllers()
    ///     .AddNewtonsoftJson(NewtonsoftSerializer.Configure);
    /// </code>
    /// </para>
    /// </remarks>
    public static void Configure(MvcNewtonsoftJsonOptions options)
    {
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
    }
}
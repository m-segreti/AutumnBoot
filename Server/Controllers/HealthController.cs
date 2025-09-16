using Microsoft.AspNetCore.Mvc;
using Server.Domain;

namespace Server.Controllers;

/// <summary>
/// Provides a simple health check endpoint for the server.
/// </summary>
[ApiController]
[Route("api/v1/health")]
public sealed class HealthController : ControllerBase
{
    /// <summary>
    /// Returns the current health status of the server.
    /// </summary>
    /// <returns>
    /// A <see cref="ServerHealth"/> object containing:
    /// <list type="bullet">
    ///   <item><c>Status</c> → A string value indicating health (e.g., "UP").</item>
    ///   <item><c>Version</c> → A string representing the API version (e.g., "v1").</item>
    /// </list>
    /// </returns>
    [HttpGet("")]
    public ServerHealth Health()
    {
        return new ServerHealth("UP", "v1");
    }
}
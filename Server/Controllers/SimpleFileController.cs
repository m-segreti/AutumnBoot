using Microsoft.AspNetCore.Mvc;
using Server.Domain;
using Server.Services;

namespace Server.Controllers;

/// <summary>
/// API controller for basic file system operations.
/// </summary>
[ApiController]
[Route("api/v1/filesystem")]
public sealed class SimpleFileController(ISimpleFileService fileService) : ControllerBase
{
    /// <summary>
    /// Saves a <see cref="SimpleFile"/> to the underlying file system.
    /// </summary>
    /// <param name="file">The <see cref="SimpleFile"/> containing the target <c>Path</c> and <c>Content</c>.</param>
    [HttpPost("file")]
    public void Save([FromBody] SimpleFile file)
    {
        fileService.Save(file.Path, file.Content);
    }
}
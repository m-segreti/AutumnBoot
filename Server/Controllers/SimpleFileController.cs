using Microsoft.AspNetCore.Mvc;
using Server.Domain;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/filesystem")]
public sealed class SimpleFileController(ISimpleFileService fileService) : ControllerBase
{
    [HttpPost("file")]
    public void Save([FromBody] SimpleFile file)
    {
        fileService.Save(file.path, file.content);
    }
}
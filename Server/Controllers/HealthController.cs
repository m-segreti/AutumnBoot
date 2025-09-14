using Microsoft.AspNetCore.Mvc;
using Server.Domain;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/health")]
public sealed class HealthController : ControllerBase
{
    [HttpGet("")]
    public ServerHealth Health()
    {
        return new ServerHealth("UP", "v1");
    }
}
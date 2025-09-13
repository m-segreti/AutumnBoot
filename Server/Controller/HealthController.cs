using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers;

[ApiController]
[Route("api/v1/health")]
public sealed class HealthController : ControllerBase {
    [HttpGet("")]
    public string Health() {
        return "UP";
    }
}

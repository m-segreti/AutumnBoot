using System;
using Microsoft.AspNetCore.Mvc;
using Server.Domain;

namespace Server.Configuration;

[ApiController]
[Route("/error")]
[ApiExplorerSettings(IgnoreApi = true)]
public sealed class ErrorController : ControllerBase
{
    [HttpGet]
    public IActionResult HandleError() {
        Exception? exception = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        if (exception is ArgumentException argEx) {
            return BadRequest(new DefaultResponse(400, argEx.Message));
        }

        return Problem(
            statusCode: 500,
            title: "An unexpected error occurred",
            detail: exception?.Message
        );
    }
}

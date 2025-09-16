using System;
using Microsoft.AspNetCore.Mvc;
using Server.Domain;

namespace Server.Configuration;

/// <summary>
/// Global error-handling controller for the application.
/// </summary>
/// <remarks>
/// <para>
/// This controller is mapped to the <c>/error</c> route and is invoked automatically by the 
/// ASP.NET Core <c>UseExceptionHandler</c> middleware when an unhandled exception occurs.
/// </para>
/// <para>
/// It distinguishes between expected exceptions (e.g., <see cref="ArgumentException"/>) 
/// and unexpected ones:
/// <list type="bullet">
///   <item>For <see cref="ArgumentException"/>: returns HTTP 400 (Bad Request) with a <see cref="DefaultResponse"/> payload.</item>
///   <item>For all other exceptions: returns HTTP 500 (Internal Server Error) with a problem details response.</item>
/// </list>
/// </para>
/// <para>
/// The <see cref="ApiExplorerSettingsAttribute.IgnoreApi"/> attribute ensures that this 
/// endpoint is excluded from generated API documentation (e.g., Swagger/OpenAPI).
/// </para>
/// </remarks>
[ApiController]
[Route("/error")]
[ApiExplorerSettings(IgnoreApi = true)]
public sealed class ErrorController : ControllerBase
{
    /// <summary>
    /// Handles errors caught by the ASP.NET Core exception handling middleware.
    /// </summary>
    /// <returns>
    /// An <see cref="IActionResult"/> that represents either:
    /// <list type="bullet">
    ///   <item><see cref="BadRequestObjectResult"/> (400) with a <see cref="DefaultResponse"/> if the error is an <see cref="ArgumentException"/>.</item>
    ///   <item><see cref="ObjectResult"/> (500) with <c>ProblemDetails</c> for unexpected exceptions.</item>
    /// </list>
    /// </returns>
    [HttpGet]
    public IActionResult HandleError() {
        // Retrieve the exception from the current HTTP context
        Exception exception = HttpContext.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        // Handle known ArgumentException cases as 400 Bad Request (This will be expanded as I support more features)
        if (exception is ArgumentException argEx) {
            return BadRequest(new DefaultResponse(400, argEx.Message));
        }

        // Fallback: return ProblemDetails (500 Internal Server Error)
        return Problem(
            statusCode: 500,
            title: "An unexpected error occurred",
            detail: exception?.Message
        );
    }
}

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ContosoRecipesApi.Controllers;

[ApiController]
public class ErrorController(ILogger<ErrorController> logger) : ControllerBase
{
    private readonly ILogger<ErrorController> _logger = logger;
    [HttpGet("/error")]
    public IActionResult Error()
    {

        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var stackTrace = context.Error.StackTrace;
        var errorMessage = context.Error.Message;

        _logger.LogError($"An error occurred: {errorMessage}\nStackTrace: {stackTrace}");

        return Problem();
    }
}

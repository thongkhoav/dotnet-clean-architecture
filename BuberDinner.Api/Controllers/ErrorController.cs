using BuberDinner.Application.Common.Errors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;
public class ErrorController : ControllerBase
{
    [Route("error")]
    public IActionResult Error()
    {
        Exception? exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        // DuplicateEmailException ==> 409 Conflict
        // Other exceptions ==> 500 Internal Server Error
        var (statusCode, message) = exception switch
        {
            IServiceException serviceException => ((int)serviceException.StatusCode, serviceException.Message),
            _ => (500, "Something went wrong")
        };
        return Problem(statusCode: statusCode, title: message);
    }
}
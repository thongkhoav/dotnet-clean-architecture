using System.Net;
using BuberDinner.Api.Http;
using BuberDinner.Application.Common.Errors;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        // add errors to BuberDinnerProblemDetailsFactory handle later
        HttpContext.Items[HttpContextItemKeys.Errors] = errors;
        var firstError = errors[0];
        var statusCode = firstError.Type switch
        {
            ErrorType.Conflict => (int)StatusCodes.Status409Conflict,
            ErrorType.Validation => (int)StatusCodes.Status400BadRequest,
            ErrorType.NotFound => (int)StatusCodes.Status404NotFound,
            _ => (int)HttpStatusCode.InternalServerError
        };
        return Problem(
            statusCode: statusCode,
            title: firstError.Description
        );
    }
}
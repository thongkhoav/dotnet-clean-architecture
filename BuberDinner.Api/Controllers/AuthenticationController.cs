using Microsoft.AspNetCore.Mvc;
namespace BuberDinner.Api.Controllers;

using System.Net;
using BuberDinner.Application.Authentication;
using BuberDinner.Application.Authentication.Commands;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using FluentResults;
using MediatR;

[Route("auth")]
public class AuthenticationController : ApiController
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(request.Email, request.Password, request.FirstName, request.LastName);
        ErrorOr<AuthenticationResult> registerResult = await _mediator.Send(command);
        return registerResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(
                errors
            )
        );
        // if (registerResult.IsSuccess)
        // {
        //     return Ok(MapAuthResult(registerResult.Value));
        // }
        // var firstError = registerResult.Errors[0];
        // if (firstError is DuplicateEmailError)
        // {
        //     return Problem(
        //         statusCode: (int)StatusCodes.Status409Conflict,
        //         title: "Email already exists"
        //     );
        // }

        // authResult = { User user, String token}
        // Match(T0: AuthenticationResult, T1: DuplicateEmailError)
        // return registerResult.Match(
        //     authResult => Ok(MapAuthResult(authResult)),
        //     error => Problem(
        //         statusCode: (int)error.StatusCode,
        //         title: error.Message
        //     )
        // );

    }

    private static AuthenticationResponse MapAuthResult(AuthenticationResult authResult) =>
        new(
        authResult.User.Id,
        authResult.User.Email,
        authResult.Token,
        authResult.User.FirstName,
        authResult.User.LastName
        );

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var loginResult = _authQueryService.Login(request.Email, request.Password);
        // authResult = { User user, String token}
        if (loginResult.IsError && loginResult.FirstError == Errors.Authentication.InvalidCredentials)
        {
            // Problem take args like this in ControllerBase
            return Problem(
                statusCode: StatusCodes.Status401Unauthorized,
                title: loginResult.FirstError.Description
            );
        }

        // Problem take in a list of errors in ApiController
        return loginResult.Match(
            authResult => Ok(MapAuthResult(authResult)),
            errors => Problem(
                errors
            )
        );
    }
}
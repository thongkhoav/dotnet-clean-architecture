using Microsoft.AspNetCore.Mvc;
namespace BuberDinner.Api.Controllers;

using System.Net;
using BuberDinner.Application.Authentication;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using FluentResults;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        ErrorOr<AuthenticationResult> registerResult = _authenticationService.Register(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        return registerResult.MatchFirst<IActionResult>(
            authResult => Ok(MapAuthResult(authResult)),
            error => Problem(
                statusCode: (int)StatusCodes.Status409Conflict,
                title: error.Description
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
        var authResult = _authenticationService.Login(request.Email, request.Password);
        // authResult = { User user, String token}
        var response = new AuthenticationResponse(authResult.User.Id, authResult.User.Email, authResult.Token, authResult.User.FirstName, authResult.User.LastName);
        return Ok(response);
    }
}
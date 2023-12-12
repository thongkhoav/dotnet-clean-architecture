using Microsoft.AspNetCore.Mvc;
namespace BuberDinner.Api.Controllers;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Contracts.Authentication;
using ErrorOr;
using FluentResults;
using MediatR;
using BuberDinner.Application.Authentication.Queries.Login;
using MapsterMapper;

[Route("auth")]
public class AuthenticationController : ApiController
{
    // IMediator contains ISender and IPublisher
    private readonly ISender _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(ISender mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        // var command = new RegisterCommand(request.Email, request.Password, request.FirstName, request.LastName);
        var command = _mapper.Map<RegisterCommand>(request);
        ErrorOr<AuthenticationResult> registerResult = await _mediator.Send(command);
        return registerResult.Match(
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(
                errors
            )
        );

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
    public async Task<IActionResult> Login(LoginRequest request)
    {
        // var query = new LoginQuery(request.Email, request.Password);
        var query = _mapper.Map<LoginQuery>(request);
        var loginResult = await _mediator.Send(query);
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
            authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
            errors => Problem(
                errors
            )
        );
    }
}
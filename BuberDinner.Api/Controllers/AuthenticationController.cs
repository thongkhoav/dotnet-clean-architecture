using Microsoft.AspNetCore.Mvc;
namespace BuberDinner.Api.Controllers;
using BuberDinner.Application.Authentication;
using BuberDinner.Contracts.Authentication;

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
        var authResult = _authenticationService.Register(request.Email, request.Password, request.FirstName, request.LastName);
        // authResult = { User user, String token}
        var response = new AuthenticationResponse(authResult.User.Id, authResult.User.Email, authResult.Token, authResult.User.FirstName, authResult.User.LastName);
        return Ok(response);
    }

    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var authResult = _authenticationService.Login(request.Email, request.Password);
        // authResult = { User user, String token}
        var response = new AuthenticationResponse(authResult.User.Id, authResult.User.Email, authResult.Token, authResult.User.FirstName, authResult.User.LastName);
        return Ok(response);
    }
}
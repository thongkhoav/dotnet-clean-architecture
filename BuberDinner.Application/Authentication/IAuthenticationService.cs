using BuberDinner.Application.Common.Errors;
using ErrorOr;
// using FluentResults;

namespace BuberDinner.Application.Authentication;

public interface IAuthenticationService
{
    ErrorOr<AuthenticationResult> Register(string email, string password, string firstName, string lastName);
    AuthenticationResult Login(string email, string password);
}
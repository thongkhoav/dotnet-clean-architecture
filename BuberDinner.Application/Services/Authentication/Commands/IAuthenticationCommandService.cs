using BuberDinner.Application.Common.Errors;
using ErrorOr;

namespace BuberDinner.Application.Authentication.Commands;

public interface IAuthenticationCommandService
{
    ErrorOr<AuthenticationResult> Register(string email, string password, string firstName, string lastName);
}
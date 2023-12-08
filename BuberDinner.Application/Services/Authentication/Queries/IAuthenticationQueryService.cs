using BuberDinner.Application.Common.Errors;
using ErrorOr;
// using FluentResults;

namespace BuberDinner.Application.Authentication.Queries;

public interface IAuthenticationQueryService
{
    ErrorOr<AuthenticationResult> Login(string email, string password);
}
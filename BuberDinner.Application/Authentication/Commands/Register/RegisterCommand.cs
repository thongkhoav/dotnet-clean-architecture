using ErrorOr;
using MediatR;

namespace BuberDinner.Application.Authentication.Commands.Register;

// same format RegisterRequest
public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<ErrorOr<AuthenticationResult>>;

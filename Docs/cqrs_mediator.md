### Step to split service:

- Do not use service, use mediator to coordinate(điều phối) depend on Query, Command
- Commands is manipulate data, Queries is read data
- Folder: Authentication > (Commands, Queries, Common)
  - Commands: RegisterCommand, RegisterCommandHandler
  - Queries: LoginQuery, LoginQueryHandler
  - Common: AuthenticationResult
- Package added:
  - dotnet add .\BuberDinner.Application\ package MediatR
- Queries:
  - LoginQuery: public record LoginQuery(
    string Email,
    string Password
    ) : IRequest`<ErrorOr<AuthenticationResult>>`;
  - LoginQueryHandler: public class LoginQueryHandler : IRequestHandler`<LoginQuery, ErrorOr<AuthenticationResult>>`
- Commands is the same
- In DependencyInjection:
  - Package add: dotnet add .\BuberDinner.Application\ package MediatR.Extensions.Microsoft.DependencyInjection
  - services.AddMediatR(typeof(DependencyInjection).Assembly);
- Use in Controller:
  - var query = new LoginQuery(request.Email, request.Password);
    var loginResult = await \_mediator.Send(query);

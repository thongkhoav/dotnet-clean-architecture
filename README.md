### BuberDinner.Api use BuberDinner.Contracts(model)

### BuberDinner.Api call BuberDinner.Application(service)

## Command:

- dotnet new sln -o BuberDinner
- cd BuberDinner
- dotnet new webapi -o BuberDinner.Api
- dotnet new classlib -o BuberDinner.Contracts
- dotnet new classlib -o BuberDinner.Infrastructure
- dotnet new classlib -o BuberDinner.Application
- dotnet new classlib -o BuberDinner.Domain
- In bash(add 5 proj to sln): dotnet sln add $(find . -name '\*.csproj')
- To test: dotnet build
- dotnet add ./BuberDinner.Api/ reference ./BuberDinner.Contracts/ ././BuberDinner.Application/
- dotnet add ./BuberDinner.Infrastructure/ reference ./BuberDinner.Application/
- dotnet add ./BuberDinner.Application/ reference ./BuberDinner.Domain/
- dotnet add ./BuberDinner.Api/ reference ./BuberDinner.Infrastructure/
- [RUN] dotnet watch run --project ./BuberDinner.Api/
- dotnet add ./BuberDinner.Application/ package Microsoft.Extensions.DependencyInjection.Abstractions

### Step to make login api:

- Define model in .Contracts
- Define Controller in .Api
- Define IService, Service in .Application (Service will not use model of .Contracts)
- Inject IService, Service by func of DependencyInjection in .Application to Controller use Dependency Injection
- Controller call service through DI

### Add Env to generate Jwt:

- Add JwtSettings object in appsettings and appsettings.Development in .Api
- Add JwtSetting class in .Infrastructure
- dotnet add ./BuberDinner.Infrastructure/ package Microsoft.Extensions.Configuration
- dotnet add ./BuberDinner.Infrastructure/ package Microsoft.Extensions.Options.ConfigurationExtensions
- [Optional] dotnet user-secrets init --project ./BuberDinner.Api/ ==> check in BuberDinner.Api.csproj - UserSecretsId
- [Optional] dotnet user-secrets set --project ./BuberDinner.Api/ "JwtSettings:SecretKey" "your_secret_key_here_greater_than_256_bits" (override in appSettings or appSettings.Development)
  - To check: dotnet user-secrets list --project ./BuberDinner.Api/
  - To remove: dotnet user-secrets remove JwtSettings:SecretKey --project ./BuberDinner.Api/

### Step to apply jwt token:

- IJwtTokenGenerator in .Application implemented by JwtTokenGenerator in .Infrastructure
- IDateTimeProvider in .Application implemented by DateTimeProvider in .Infrastructure (this use in JwtTokenGenerator)
- dotnet add ./BuberDinner.Infrastructure/ package System.IdentityModel.Tokens.Jwt
- Add Singleton Jwt in .Infrastructure

### Repository pattern

- Define User in .Domain (inside Entities folder)
- Define IUserRepository in .Application (used in AuthenticationService)
- Define UserRepository in .Infrastructure
- services.AddScoped<IUserRepository, UserRepository>(); in .Infrastructure

### Error handling

- ErrorHandlingMiddleware

  - Define ExceptionHandlerMiddleware in .Api
  - Add to Program.cs in .Api: app.UseMiddleware<ExceptionHandlerMiddleware>();

- ErrorHandlingFilterAttribute (catch exception and format response)

  - Define ErrorHandlingFilterAttribute in .Api
  - Add to Program.cs in .Api: builder.Services.AddControllers(options => options.Filters.Add<ErrorHandlingFilterAttribute>());

- Return error in Service

  - dotnet add .\BuberDinner.Application\ package oneof
  - public record struct DuplicateEmailError(); in .Application
  - change OneOf<AuthenticationResult, DuplicateEmailError> Register( in IAuthenticationService
  - change AuthenticationService and AuthenticationController
  - Other way: dotnet add .\BuberDinner.Application\ package fluentResults

- Define error in .Domain layer
  - dotnet add .\BuberDinner.Domain\ package ErrorOr
  - ErrorOr<AuthenticationResult> can result in AuthenticationResult or list<error>
  - Define and custom Problem method in ApiController:ControllerBase

### Debugg .Net Attach

- Ctrl Shift P ==> .Net: Generate Assets ...
- To use ".Net Core Attach": run project first.

### CQRS: Command(Manipulate data) && Query(Read data)

- CommandService and QueryService
- Controller call [Mediator], Mediator will điều phối(coordinate) to command or query service
  - Add package: dotnet add .\BuberDinner.Application\ package MediatR

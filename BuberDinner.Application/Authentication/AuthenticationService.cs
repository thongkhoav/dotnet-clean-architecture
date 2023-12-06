
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistencek;
using BuberDinner.Domain.Entities;

namespace BuberDinner.Application.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public AuthenticationResult Login(string email, string password)
    {
        // User exist will continue
        var user = _userRepository.GetUserByEmail(email);
        if (user is null)
        {
            throw new Exception("User does not exist");
        }

        // Password is correct will continue
        if (user.Password != password)
        {
            throw new Exception("Password is incorrect");
        }

        // Generate token
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }

    public AuthenticationResult Register(string email, string password, string firstName, string lastName)
    {
        // User do not exist will continue
        if (_userRepository.GetUserByEmail(email) is not null)
        {
            throw new Exception("User already exists");
        }
        // Create user
        var user = new User
        {
            Email = email,
            Password = password,
            FirstName = firstName,
            LastName = lastName

        };
        _userRepository.Add(user);

        // Generate token
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new AuthenticationResult(user, token);
    }
}

namespace BuberDinner.Infrastructure.Authentication;

public class JwtSettings
{
    public const string sectionName = "JwtSettings";
    public string SecretKey { get; init; } = null!;
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public int ExpirationInMinutes { get; init; }
}

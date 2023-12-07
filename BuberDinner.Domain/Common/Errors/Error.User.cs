using ErrorOr;

namespace BuberDinner.Application.Common.Errors;

public static class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "duplicate_email",
            description: "Email already exists"
        );
    }
}

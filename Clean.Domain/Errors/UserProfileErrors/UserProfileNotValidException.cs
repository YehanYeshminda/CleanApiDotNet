using Clean.Domain.Errors.ValidityExceptions;

namespace Clean.Domain.Errors.UserProfileErrors;

public class UserProfileNotValidException : NotValidException
{
    internal UserProfileNotValidException() { }
    internal UserProfileNotValidException(string message) : base(message) { }
    internal UserProfileNotValidException(string message, Exception innerException) : base(message, innerException) { }
}
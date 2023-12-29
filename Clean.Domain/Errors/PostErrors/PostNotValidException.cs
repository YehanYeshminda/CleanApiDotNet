using Clean.Domain.Errors.ValidityExceptions;

namespace Clean.Domain.Errors.PostErrors;

public class PostNotValidException : NotValidException
{
    internal PostNotValidException() { }
    internal PostNotValidException(string message) : base(message) { }
    internal PostNotValidException(string message, Exception innerException) : base(message, innerException) { }
}
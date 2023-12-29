using Clean.Domain.Errors.ValidityExceptions;

namespace Clean.Domain.Errors.PostErrors;

public class PostCommentNotValidateException : NotValidException
{
    internal PostCommentNotValidateException() { }
    internal PostCommentNotValidateException(string message) : base(message) { }
    internal PostCommentNotValidateException(string message, Exception innerException) : base(message, innerException) { }
}
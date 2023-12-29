namespace Clean.Domain.Errors.ValidityExceptions;

public class NotValidException : Exception
{
    internal NotValidException()
    {
        ValidationErrors = new List<string>();
    }

    internal NotValidException(string message) : base(message)
    {
        ValidationErrors = new List<string>();
    }

    internal NotValidException(string message, Exception innerException) : base(message, innerException)
    {
        ValidationErrors = new List<string>();
    }
    
    public List<string> ValidationErrors { get; }
}
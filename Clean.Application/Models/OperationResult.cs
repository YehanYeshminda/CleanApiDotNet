namespace Clean.Application.Models;

public class OperationResult<T>
{
    public T Payload { get; set; }
    public bool IsError { get; set; } = false;
    public List<Error> Errors { get; } = new List<Error>();
}
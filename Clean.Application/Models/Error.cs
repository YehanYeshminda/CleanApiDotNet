using Clean.Application.Enums;

namespace Clean.Application.Models;

public class Error
{
    public ErrorCodes Code { get; set; }
    public string Message { get; set; }
}
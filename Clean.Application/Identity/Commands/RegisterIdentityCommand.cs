using Clean.Application.Models;
using MediatR;

namespace Clean.Application.Identity.Commands;

public class RegisterIdentityCommand : IRequest<OperationResult<string>>
{
    public string EmailAdress { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string CurrentCity { get; set; }
}
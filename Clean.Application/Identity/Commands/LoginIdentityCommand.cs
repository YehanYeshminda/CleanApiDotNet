using System.ComponentModel.DataAnnotations;
using Clean.Application.Models;
using MediatR;

namespace Clean.Application.Identity.Commands;

public class LoginIdentityCommand : IRequest<OperationResult<string>>
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}
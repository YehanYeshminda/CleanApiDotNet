using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Clean.API.Contracts.Identity;

public class LoginContract
{
    [EmailAddress]
    [Required]
    public string EmailAdress { get; set; }
    
    [Required]
    public string Password { get; set; }
}
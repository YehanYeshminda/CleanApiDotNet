using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Clean.Application.Enums;
using Clean.Application.Identity.Commands;
using Clean.Application.Models;
using Clean.Application.Services;
using Clean.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Identity.Handlers;

public class LoginIdentityCommandHandler : IRequestHandler<LoginIdentityCommand, OperationResult<string>>
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;
    public LoginIdentityCommandHandler(DataContext context, UserManager<IdentityUser> userManager,IdentityService identityService)
    {
        _context = context;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    public async Task<OperationResult<string>> Handle(LoginIdentityCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();
        
        try
        {
            var user = new IdentityUser()
            {
                Email = request.Email,
                UserName = request.Email
            };
            
            var existingIdentity = await _userManager.FindByEmailAsync(request.Email);
            
            if (existingIdentity is null)
            {
                var error = new Error { Code = ErrorCodes.InvalidPasswordEmail, Message = $"Email or password is wrong. Login failed."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }
            
            var isPasswordValid = await _userManager.CheckPasswordAsync(existingIdentity, request.Password);
            
            if (isPasswordValid == false)
            {
                var error = new Error { Code = ErrorCodes.InvalidPasswordEmail, Message = $"Email or password is wrong. Login failed."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }
            
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.IdentityId == existingIdentity.Id);
            
            if (userProfile is null)
            {
                var error = new Error { Code = ErrorCodes.InvalidPasswordEmail, Message = $"Email or password is wrong. Login failed."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }

            var claimsIdentity = new ClaimsIdentity(new Claim[]
            {
                new Claim("Id", existingIdentity.Id),
                new Claim(JwtRegisteredClaimNames.Sub, existingIdentity.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, existingIdentity.Email),
                new Claim("UserProfileId", userProfile.UserProfileId.ToString())
            });

            var token = _identityService.CreateSecurityToken(claimsIdentity);
            result.Payload = _identityService.WriteToken(token);
            return result;
        }
        catch (Exception ex)
        {
            var error = new Error { Code = ErrorCodes.ServerError, Message = $"{ex.Message}"};
            result.Errors.Add(error);
            result.IsError = true;
            return result;
        }
    }
}
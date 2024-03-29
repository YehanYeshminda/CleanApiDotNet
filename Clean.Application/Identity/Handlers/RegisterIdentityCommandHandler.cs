using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clean.Application.Enums;
using Clean.Application.Identity.Commands;
using Clean.Application.Models;
using Clean.Application.Options;
using Clean.Application.Services;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Application.Identity.Handlers;

public class RegisterIdentityCommandHandler : IRequestHandler<RegisterIdentityCommand, OperationResult<string>>
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityService _identityService;

    public RegisterIdentityCommandHandler(DataContext context, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings, IdentityService identityService)
    {
        _context = context;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    public async Task<OperationResult<string>> Handle(RegisterIdentityCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<string>();

        try
        {
            var existingIdentity = await _userManager.FindByEmailAsync(request.EmailAdress);
            
            if (existingIdentity != null)
            {
                var error = new Error { Code = ErrorCodes.ExistingIdentityUser, Message = $"Identity with email {request.EmailAdress} already exists."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }

            var identity = new IdentityUser
            {
                Email = request.EmailAdress,
                UserName = request.EmailAdress
            };

            using var transaction = _context.Database.BeginTransaction();
            
            var profileInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAdress, request.Phone, request.DateOfBirth, request.CurrentCity);
            
            if (profileInfo == null)
            {
                var error = new Error { Code = ErrorCodes.BadRequest, Message = $"User profile is not valid."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }
            
            var userProfile = UserProfile.CreateUserProfile(identity.Id, profileInfo);
            
            if (userProfile == null)
            {
                var error = new Error { Code = ErrorCodes.BadRequest, Message = $"User profile is not valid."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }
            
            try
            {
                var identityResult = await _userManager.CreateAsync(identity, request.Password);
                
                if (!identityResult.Succeeded)
                {
                    result.IsError = true;
                    foreach (var identityError in identityResult.Errors)
                    {
                        var error = new Error { Code = ErrorCodes.IdentityUserCreationFailed, Message = $"{identityError.Description}"};
                        result.Errors.Add(error);
                    }
                    
                    await transaction.RollbackAsync();
                    return result;
                }
            
                await _context.UserProfiles.AddAsync(userProfile);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
            
            await transaction.CommitAsync();
            
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
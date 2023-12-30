using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clean.Application.Enums;
using Clean.Application.Identity.Commands;
using Clean.Application.Models;
using Clean.Application.Options;
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
    private readonly JwtSettings _jwtSettings;

    public RegisterIdentityCommandHandler(DataContext context, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
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
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", identity.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, identity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, identity.Email),
                    new Claim("UserProfileId", userProfile.UserProfileId.ToString())
                }),
                
                Expires = DateTime.Now.AddHours(9),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtSettings.Audience[0],
                Issuer = _jwtSettings.Issuer
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            
            result.Payload = tokenString;
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
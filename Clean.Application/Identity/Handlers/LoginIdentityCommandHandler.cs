using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Clean.Application.Enums;
using Clean.Application.Identity.Commands;
using Clean.Application.Models;
using Clean.Application.Options;
using Clean.DAL;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Clean.Application.Identity.Handlers;

public class LoginIdentityCommandHandler : IRequestHandler<LoginIdentityCommand, OperationResult<string>>
{
    private readonly DataContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly JwtSettings _jwtSettings;

    public LoginIdentityCommandHandler(DataContext context, UserManager<IdentityUser> userManager, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _userManager = userManager;
        _jwtSettings = jwtSettings.Value;
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
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SigningKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", existingIdentity.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, existingIdentity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, existingIdentity.Email),
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
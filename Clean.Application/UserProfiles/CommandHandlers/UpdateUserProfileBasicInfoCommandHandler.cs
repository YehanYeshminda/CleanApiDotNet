using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.UserProfiles.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using Clean.Domain.Errors.UserProfileErrors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileBasicInfoCommandHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _context;
    public UpdateUserProfileBasicInfoCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken cancellationToken)
    {
            var result = new OperationResult<UserProfile>();
        try
        {
            var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == request.UserProfileId);
            
            if (userProfile is null)
            {
                result.IsError = true;
                var error = new Error { Code = ErrorCodes.NotFound, Message = $"User profile not found with id {request.UserProfileId}" };
                result.Errors.Add(error);
                return result;
            }
            
            var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);
        
            userProfile.UpdateBasicInfo(basicInfo);
            _context.UserProfiles.Update(userProfile);
        
            await _context.SaveChangesAsync();
            
            result.Payload = userProfile;
            return result;
        }
        catch (UserProfileNotValidException ex)
        {
            result.IsError = true;
            foreach (var exValidationError in ex.ValidationErrors)
            {
                var error = new Error { Code = ErrorCodes.ValidationError, Message = $"{ex.Message}"};
                result.Errors.Add(error);
            }
            
            return result;
        }
    }
}
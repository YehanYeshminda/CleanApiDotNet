using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.UserProfiles.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.UserProfiles.CommandHandlers;

public class DeleteUserProfileCommandHandler : IRequestHandler<DeleteUserProfileCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _context;

    public DeleteUserProfileCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(DeleteUserProfileCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == request.UserProfileId);
        
        if (userProfile is null)
        {
            result.IsError = true;
            var error = new Error { Code = ErrorCodes.NotFound, Message = $"User profile not found with id {request.UserProfileId}" };
            result.Errors.Add(error);
            return result;
        }
        
        _context.UserProfiles.Remove(userProfile);
        await _context.SaveChangesAsync();
        
        result.Payload = userProfile;
        return result;
    }
}
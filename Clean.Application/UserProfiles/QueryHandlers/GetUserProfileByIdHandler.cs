using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.UserProfiles.Queries;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileById, OperationResult<UserProfile>>
{
    private readonly DataContext _context;

    public GetUserProfileByIdHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(GetUserProfileById request, CancellationToken cancellationToken)
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
            
        result.Payload = userProfile;
        return result;
    }
}
using Clean.Application.Models;
using Clean.Application.UserProfiles.Queries;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.UserProfiles.QueryHandlers;

internal class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfiles, OperationResult<IEnumerable<UserProfile>>>
{
    private readonly DataContext _context;

    public GetAllUserProfilesQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<IEnumerable<UserProfile>>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<IEnumerable<UserProfile>>();
        result.Payload = await _context.UserProfiles.ToListAsync();;
        return result;
    }
}
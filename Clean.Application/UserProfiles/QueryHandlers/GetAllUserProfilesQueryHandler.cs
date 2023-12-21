using Clean.Application.UserProfiles.Queries;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.UserProfiles.QueryHandlers;

internal class GetAllUserProfilesQueryHandler : IRequestHandler<GetAllUserProfiles, IEnumerable<UserProfile>>
{
    private readonly DataContext _context;

    public GetAllUserProfilesQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<UserProfile>> Handle(GetAllUserProfiles request, CancellationToken cancellationToken)
    {
        return await _context.UserProfiles.ToListAsync();
    }
}
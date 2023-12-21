using Clean.Application.UserProfiles.Queries;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.UserProfiles.QueryHandlers;

internal class GetUserProfileByIdHandler : IRequestHandler<GetUserProfileById, UserProfile>
{
    private readonly DataContext _context;

    public GetUserProfileByIdHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<UserProfile> Handle(GetUserProfileById request, CancellationToken cancellationToken)
    {
        return await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == request.UserProfileId);
    }
}
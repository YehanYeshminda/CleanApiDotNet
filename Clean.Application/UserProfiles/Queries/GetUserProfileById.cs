using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Clean.Application.UserProfiles.Queries;

public class GetUserProfileById : IRequest<UserProfile>
{
    public Guid UserProfileId { get; set; }
}
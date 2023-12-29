using Clean.Application.Models;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Clean.Application.UserProfiles.Queries;

public class GetUserProfileById : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
}
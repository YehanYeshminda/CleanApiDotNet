using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Clean.Application.UserProfiles.Queries;

public class GetAllUserProfiles : IRequest<IEnumerable<UserProfile>>
{
}
using Clean.Application.Models;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Clean.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
}
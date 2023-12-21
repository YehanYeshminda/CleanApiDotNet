using MediatR;

namespace Clean.Application.UserProfiles.Commands;

public class DeleteUserProfileCommand : IRequest
{
    public Guid UserProfileId { get; set; }
}
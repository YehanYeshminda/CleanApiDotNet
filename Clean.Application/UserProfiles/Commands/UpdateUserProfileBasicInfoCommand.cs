using Clean.Application.Models;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Clean.Application.UserProfiles.Commands;

public class UpdateUserProfileBasicInfoCommand : IRequest<OperationResult<UserProfile>>
{
    public Guid UserProfileId { get; set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string EmailAddress { get; private set; }
    public string Phone { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string CurrentCity { get; private set; }
}
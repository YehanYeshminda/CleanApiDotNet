using AutoMapper;
using Clean.Application.UserProfiles.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Clean.Application.UserProfiles.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserProfile>
{
    private readonly DataContext _context;

    public CreateUserCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
    }
    
    public async Task<UserProfile> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);
        var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

        _context.UserProfiles.Add(userProfile);
        await _context.SaveChangesAsync(cancellationToken);
        
        return userProfile;
    }
}
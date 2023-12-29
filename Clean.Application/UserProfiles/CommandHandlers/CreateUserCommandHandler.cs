using AutoMapper;
using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.UserProfiles.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;

namespace Clean.Application.UserProfiles.CommandHandlers;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<UserProfile>>
{
    private readonly DataContext _context;

    public CreateUserCommandHandler(DataContext context, IMapper mapper)
    {
        _context = context;
    }
    
    public async Task<OperationResult<UserProfile>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<UserProfile>();
        
        var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);
        var userProfile = UserProfile.CreateUserProfile(Guid.NewGuid().ToString(), basicInfo);

        _context.UserProfiles.Add(userProfile);
        await _context.SaveChangesAsync(cancellationToken);
            
        result.Payload = userProfile;

        return result;
    }
}
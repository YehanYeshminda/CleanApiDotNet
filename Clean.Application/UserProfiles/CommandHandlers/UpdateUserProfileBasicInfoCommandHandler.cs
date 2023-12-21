using Clean.Application.UserProfiles.Commands;
using Clean.DAL;
using Clean.Domain.Aggregates.UserProfileAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.UserProfiles.CommandHandlers;

internal class UpdateUserProfileBasicInfoCommandHandler : IRequestHandler<UpdateUserProfileBasicInfoCommand>
{
    private readonly DataContext _context;

    public UpdateUserProfileBasicInfoCommandHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(UpdateUserProfileBasicInfoCommand request, CancellationToken cancellationToken)
    {
        var userProfile = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserProfileId == request.UserProfileId);
        var basicInfo = BasicInfo.CreateBasicInfo(request.FirstName, request.LastName, request.EmailAddress, request.Phone, request.DateOfBirth, request.CurrentCity);
        
        userProfile.UpdateBasicInfo(basicInfo);
        _context.UserProfiles.Update(userProfile);
        
        await _context.SaveChangesAsync();
        
        return new Unit();
    }
}
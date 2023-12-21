using AutoMapper;
using Clean.Application.UserProfiles.Commands;
using Clean.Domain.Aggregates.UserProfileAggregate;

namespace Clean.Application.MappingProfiles;

internal class UserProfileMap : Profile
{
    public UserProfileMap()
    {
        CreateMap<CreateUserCommand, BasicInfo>();
    }
}
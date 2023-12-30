using AutoMapper;
using Clean.API.Contracts.Identity;
using Clean.Application.Identity.Commands;

namespace Clean.API.MappingProfiles;

public class IdentityMappings : Profile
{
    public IdentityMappings()
    {
        CreateMap<UserRegistrationContract, RegisterIdentityCommand>();
    }
}
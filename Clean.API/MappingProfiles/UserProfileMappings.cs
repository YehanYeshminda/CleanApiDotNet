using AutoMapper;
using Clean.API.Contracts.UserProfile.Requests;
using Clean.API.Contracts.UserProfile.Responses;
using Clean.Application.UserProfiles.Commands;
using Clean.Domain.Aggregates.UserProfileAggregate;

namespace Clean.API.MappingProfiles;

public class UserProfileMappings : Profile
{
    public UserProfileMappings()
    {
        // User profile mappings
        CreateMap<UserProfileCreateUpdate, UpdateUserProfileBasicInfoCommand>();
        CreateMap<UserProfileCreateUpdate, CreateUserCommand>();
        CreateMap<UserProfile, UserProfileResponse>();
        CreateMap<BasicInfo, BasicInfoResponse>();
    }
}
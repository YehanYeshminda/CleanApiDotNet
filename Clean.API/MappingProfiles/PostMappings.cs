using AutoMapper;
using Clean.API.Contracts.Post.Responses;
using Clean.Domain.Aggregates.PostAggregate;

namespace Clean.API.MappingProfiles;

public class PostMappings : Profile
{
    public PostMappings()
    {
        CreateMap<Post, PostResponse>();
    }
}
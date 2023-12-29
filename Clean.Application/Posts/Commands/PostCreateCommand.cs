using Clean.Application.Models;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Clean.Application.Posts.Commands;

public class PostCreateCommand : IRequest<OperationResult<Post>>
{
    public Guid UserProfileId { get; set; }
    public string TextContext { get; set; }
}
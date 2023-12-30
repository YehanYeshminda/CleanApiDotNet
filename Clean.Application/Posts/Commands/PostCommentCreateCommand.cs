using Clean.Application.Models;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Clean.Application.Posts.Commands;

public class PostCommentCreateCommand : IRequest<OperationResult<PostComment>>
{
    public Guid PostId { get; set; }
    public Guid UserProfileId { get; set; }
    public string TextContext { get; set; }
}
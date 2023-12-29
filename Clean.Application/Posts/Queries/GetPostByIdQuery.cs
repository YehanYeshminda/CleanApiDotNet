using Clean.Application.Models;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Clean.Application.Posts.Queries;

public class GetPostByIdQuery : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
}
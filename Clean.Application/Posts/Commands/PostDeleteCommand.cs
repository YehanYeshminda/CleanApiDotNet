using Clean.Application.Models;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Clean.Application.Posts.Commands;

public class PostDeleteCommand : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
}
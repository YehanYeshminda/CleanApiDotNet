using Clean.Application.Models;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Clean.Application.Posts.Commands;

public class PostUpdateCommand : IRequest<OperationResult<Post>>
{
    public Guid PostId { get; set; }
    public string TextContext { get; set; }
}
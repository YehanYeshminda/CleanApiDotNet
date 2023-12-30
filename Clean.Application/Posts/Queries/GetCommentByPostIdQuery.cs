using Clean.Application.Models;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Clean.Application.Posts.Queries;

public class GetCommentByPostIdQuery : IRequest<OperationResult<List<PostComment>>>
{
    public Guid PostId { get; set; }
}
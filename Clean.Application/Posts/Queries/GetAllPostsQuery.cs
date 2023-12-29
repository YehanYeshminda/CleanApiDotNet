using Clean.Application.Models;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;

namespace Clean.Application.Posts.Queries;

public class GetAllPostsQuery : IRequest<OperationResult<List<Post>>>
{
}
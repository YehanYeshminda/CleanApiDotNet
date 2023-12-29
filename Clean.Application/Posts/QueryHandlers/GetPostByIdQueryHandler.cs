using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.Posts.Queries;
using Clean.DAL;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Posts.QueryHandlers;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, OperationResult<Post>>
{
    private readonly DataContext _context;

    public GetPostByIdQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<Post>> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<Post>();

        try
        {
            var posts = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == request.PostId);
            
            if (posts is null)
            {
                var error = new Error { Code = ErrorCodes.NotFound, Message = $"Post with id {request.PostId} not found"};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }
            
            result.Payload = posts;
            return result;
        }
        catch (Exception e)
        {
            var error = new Error { Code = ErrorCodes.ServerError, Message = $"{e.Message}"};
            result.Errors.Add(error);
            result.IsError = true;
            return result;
        }
    }
}
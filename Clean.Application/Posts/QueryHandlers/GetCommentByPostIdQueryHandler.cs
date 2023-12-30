using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.Posts.Queries;
using Clean.DAL;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Posts.CommandHandlers;

public class GetCommentByPostIdQueryHandler : IRequestHandler<GetCommentByPostIdQuery, OperationResult<List<PostComment>>>
{
    private readonly DataContext _context;

    public GetCommentByPostIdQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<List<PostComment>>> Handle(GetCommentByPostIdQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<PostComment>>();
        
        try
        {
            var post = await _context.Posts.Include(x => x.Comments).FirstOrDefaultAsync(x => x.PostId == request.PostId);
            
            if (post is null)
            {
                var error = new Error { Code = ErrorCodes.NotFound, Message = $"Post with id {request.PostId} not found."};
                result.Errors.Add(error);
                result.IsError = true;
                return result;
            }

            result.Payload = post.Comments.ToList();
            return result;
        }
        catch (Exception ex)
        {
            var error = new Error { Code = ErrorCodes.ServerError, Message = $"{ex.Message}"};
            result.Errors.Add(error);
            result.IsError = true;
            return result;
        }
    }
}
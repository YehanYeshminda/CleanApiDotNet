using Clean.Application.Enums;
using Clean.Application.Models;
using Clean.Application.Posts.Queries;
using Clean.DAL;
using Clean.Domain.Aggregates.PostAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clean.Application.Posts.QueryHandlers;

public class GetAllPostQueryHandler : IRequestHandler<GetAllPostsQuery, OperationResult<List<Post>>>
{
    private readonly DataContext _context;

    public GetAllPostQueryHandler(DataContext context)
    {
        _context = context;
    }
    
    public async Task<OperationResult<List<Post>>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
    {
        var result = new OperationResult<List<Post>>();
        
        try
        {
            var posts = await _context.Posts.ToListAsync();
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
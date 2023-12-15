using Clean.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V1;

[ApiVersion(("1.0"))]
[Route("api/v{version:apiVersion}/post")]
[ApiController]
public class PostsController : Controller
{
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var post = new Post
        {
            Id = id,
            Text = "Hello world"
        };

        return Ok(post);
    }
}
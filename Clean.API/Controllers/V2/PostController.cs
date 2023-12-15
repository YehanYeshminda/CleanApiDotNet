using Clean.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V2;

[ApiVersion(("2.0"))]
[Route("api/v{version:apiVersion}/post")]
[ApiController]
public class PostController : Controller
{
    [HttpGet]
    [Route("{id}")]
    public IActionResult GetById(int id)
    {
        var post = new Post
        {
            Id = id,
            Text = "Hello world v2"
        };

        return Ok(post);
    }
}
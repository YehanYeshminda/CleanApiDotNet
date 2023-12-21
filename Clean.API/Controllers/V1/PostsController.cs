using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V1;

[ApiVersion(("1.0"))]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class PostsController : Controller
{
    [HttpGet]
    [Route(ApiRoutes.Posts.IdRoute)]
    public IActionResult GetById(int id)
    {
        return Ok();
    }
}
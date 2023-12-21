using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V2;

[ApiVersion(("2.0"))]
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
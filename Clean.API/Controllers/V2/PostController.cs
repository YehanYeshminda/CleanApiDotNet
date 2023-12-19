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
        return Ok();
    }
}
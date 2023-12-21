using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V2;

[ApiVersion(("2.0"))]
[Route(ApiRoutes.BaseRoute)]
[ApiController]
public class UserProfileController : Controller
{
    public UserProfileController()
    {
    }
    
    [HttpGet]
    [Route(ApiRoutes.UserProfiles.IdRoute)]
    public async Task<IActionResult> GetById(int id)
    {
        return (IActionResult) Task.FromResult(Ok());
    }
}
using Clean.API.Contracts.Common;
using Clean.Application.Enums;
using Clean.Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace Clean.API.Controllers.V1;

public class BaseController : ControllerBase
{
    protected IActionResult HandleErrorResponse(List<Error> errors)
    {
        var apiError = new ErrorResponse();
        
        if (errors.Any(x => x.Code == ErrorCodes.NotFound))
        {
            var error = errors.FirstOrDefault(x => x.Code == ErrorCodes.NotFound);
            
            apiError.StatusCode = 404;
            apiError.StatusPhase = "Not Found";
            apiError.TimeStamp = DateTime.UtcNow;
            apiError.Errors.Add(error.Message);
            
            return NotFound(apiError);
        }
        
        if (errors.Any(x => x.Code == ErrorCodes.ServerError))
        {
            var error = errors.FirstOrDefault(x => x.Code == ErrorCodes.ServerError);
            
            apiError.StatusCode = 500;
            apiError.StatusPhase = "Internal Server Error";
            apiError.TimeStamp = DateTime.UtcNow;
            apiError.Errors.Add(error.Message);
            
            return StatusCode(500, apiError);
        }
        
        return BadRequest(errors);
    }
}
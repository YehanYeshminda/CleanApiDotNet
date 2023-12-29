using Clean.API.Contracts.Common;
using Clean.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clean.API.Filters;

public class GeneralExceptionHandler : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var apiError = new ErrorResponse();

        apiError.StatusCode = (int)ErrorCodes.ServerError;
        apiError.StatusPhase = "Internal Server Error";
        apiError.TimeStamp = DateTime.Now;

        apiError.Errors.Add(context.Exception.Message);

        context.Result = new BadRequestObjectResult(apiError);
    }
}
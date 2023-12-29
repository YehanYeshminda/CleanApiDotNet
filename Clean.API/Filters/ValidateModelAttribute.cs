using Clean.API.Contracts.Common;
using Clean.Application.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Clean.API.Filters;

public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var apiError = new ErrorResponse();

            apiError.StatusCode = (int)ErrorCodes.BadRequest;
            apiError.StatusPhase = "Bad Request";
            apiError.TimeStamp = DateTime.Now;

            foreach (var modelState in context.ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    apiError.Errors.Add(error.ErrorMessage);
                }
            }

            context.Result = new BadRequestObjectResult(apiError);
        }
    }
}
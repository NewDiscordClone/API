using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sparkle.Application.Common.Exceptions;

namespace Sparkle.WebApi.Attributes
{
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {

            Exception exception = context.Exception;
            (int code, string message) = exception switch
            {
                InvalidOperationException or ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                EntityNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                NoPermissionsException => (StatusCodes.Status403Forbidden, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An error occurred while processing your request")
            };

            ProblemDetails problemDetails = new()
            {
                Status = code,
                Title = message,
            };
            context.Result = new ObjectResult(problemDetails);

            context.ExceptionHandled = true;

            await base.OnExceptionAsync(context);
        }
    }
}

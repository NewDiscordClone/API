using Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Attributes
{
    public class ExceptionFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                await next();
            }
            catch (NoPermissionsException exception)
            {
                context.Result = new ForbidResult(exception.Message);
            }
            catch (InvalidOperationException)
            {
                context.Result = new BadRequestResult();
            }
            catch (EntityNotFoundException ex)
            {
                context.Result = new NotFoundObjectResult(ex.Id);
            }
        }
    }
}

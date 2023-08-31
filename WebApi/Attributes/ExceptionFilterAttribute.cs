using Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Attributes
{
    public class ExceptionFilterAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            context.Result = context.Exception switch
            {
                NoPermissionsException noPermissionsException => new ForbidResult(noPermissionsException.Message),
                InvalidOperationException or ArgumentException => new BadRequestResult(),
                EntityNotFoundException entityNotFoundException => new BadRequestObjectResult(entityNotFoundException.Id),
                _ => throw context.Exception
            };

            await base.OnExceptionAsync(context);
        }
    }
}

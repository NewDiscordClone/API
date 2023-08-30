using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Attributes
{
    public partial class ServerAuthorizeAttribute : ActionFilterAttribute
    {
        public string? Policy { get; set; }

        private string GetServerId(ActionExecutingContext context)
        {
            InvalidOperationException exception = new("Server id not found in request");

            foreach (object? item in context.ActionArguments.Values)
            {
                if (item is not null && item is IServerRequest request)
                {
                    return request.ServerId;
                }
            }

            throw exception;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string id = GetServerId(context);
            IAuthorizationService authorizationService = context.HttpContext.RequestServices.GetService<IAuthorizationService>()
                ?? throw new InvalidOperationException();

            if (Policy is null)
            {
                throw new Exception();
            }
            AuthorizationResult result = await authorizationService
                .AuthorizeAsync(context.HttpContext.User, id, Policy);

            if (result.Succeeded)
            {
                await next();
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }
    }
}

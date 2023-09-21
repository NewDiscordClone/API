using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sparkle.WebApi.Attributes
{
    public partial class ServerAuthorizeAttribute : ActionFilterAttribute
    {
        public string? Policy { get; set; }

        private static string GetServerId(ActionExecutingContext context)
        {
            InvalidOperationException exception = new("Server id not found in route");

            if (!context.RouteData.Values.TryGetValue("serverId", out object? serverId))
            {
                throw exception;
            }

            if (serverId is string id)
            {
                return id;
            }
            else
            {
                throw exception;
            }
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

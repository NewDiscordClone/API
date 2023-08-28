using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text.RegularExpressions;

namespace WebApi.Attributes
{
    public partial class ServerAuthorizeAttribute : ActionFilterAttribute
    {
        public string? Policy { get; set; }

        private int GetServerId(ActionExecutingContext context)
        {
            InvalidOperationException exception = new("Server id not found in request");
            foreach (object? item in context.ActionArguments.Values)
            {
                if (item == null)
                    continue;

                Type type = item.GetType();
                PropertyInfo? idProperty = type.GetProperty("ServerId");
                if (idProperty is not null)
                {
                    int serverId = (int?)idProperty.GetValue(item)
                        ?? throw exception;
                    return serverId;
                }
            }
            throw exception;
        }

        [GeneratedRegex("[0-9]+$")]
        private static partial Regex GetServerIdFromPolicyName();



        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {

            int id = GetServerId(context);
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

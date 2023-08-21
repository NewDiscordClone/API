using Application.Providers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace WebApi.Attributes
{
    public class InRoleAttribute : ActionFilterAttribute
    {
        private IAuthorizedUserProvider _userProvider;
        private string RoleName { get; set; }

        public InRoleAttribute(string roleName)
        {
            RoleName = roleName;
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _userProvider = context.HttpContext.RequestServices.GetService<IAuthorizedUserProvider>()
                ?? throw new InvalidOperationException("The IAuthorizedUserProvider service was not registered.");

            int serverId = GetServerId(context);

            bool authorized = await _userProvider.IsInRoleAsync(RoleName, serverId);
            if (authorized)
            {
                await next();
            }
            else
            {
                context.Result = new ForbidResult();
            }
        }

        private static int GetServerId(ActionExecutingContext context)
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
    }
}

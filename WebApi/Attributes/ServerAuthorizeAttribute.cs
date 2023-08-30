using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

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
                if (item is not null)
                {
                    if (item is IServerRequest request)
                    {
                        return request.ServerId;
                    }

                    Type type = item.GetType();
                    Type iServerRequest = type.FindInterfaces((i, filter) =>
                        i.IsGenericType &&
                        i.GetGenericTypeDefinition() == typeof(IServerRequest<>), true)
                        .First();

                    if (iServerRequest is not null)
                    {
                        PropertyInfo? prop = iServerRequest.GetProperty("ServerId");

                        return prop?.GetValue(item)?.ToString()
                            ?? throw exception;
                    }
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using System.Text.RegularExpressions;
using WebApi.Providers;

namespace WebApi.Attributes
{
    public partial class ServerAuthorizeAttribute : AuthorizeAttribute, IActionFilter
    {
        public int ServerId
        {
            get
            {
                if (Policy is null)
                    return default;
                string stringId = GetServerIdFromPolicyName()
                    .Match(Policy).Value;
                if (int.TryParse(stringId, out int serverId))
                {
                    return serverId;
                }
                return default;
            }
            set
            {
                if (Policy is null)
                {
                    Policy = $"{ServerPolicies.ServerMember}{value}";
                    return;
                }
                Match match = GetServerIdFromPolicyName().Match(Policy);
                if (match.Success)
                {
                    Policy = Policy.Remove(match.Index, match.Length);
                }
                Policy = $"{Policy}{value}";
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

        [GeneratedRegex("[0-9]+$")]
        private static partial Regex GetServerIdFromPolicyName();

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ServerId = GetServerId(context);
        }
    }
}

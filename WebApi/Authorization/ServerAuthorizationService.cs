using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Sparkle.WebApi.Authorization
{
    public class ServerAuthorizationService : DefaultAuthorizationService
    {
        public ServerAuthorizationService(IAuthorizationPolicyProvider policyProvider, IAuthorizationHandlerProvider handlers, ILogger<DefaultAuthorizationService> logger, IAuthorizationHandlerContextFactory contextFactory, IAuthorizationEvaluator evaluator, IOptions<AuthorizationOptions> options) : base(policyProvider, handlers, logger, contextFactory, evaluator, options)
        {
        }

        public override Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object? resource, string policyName)
        {
            if (resource is Guid profileId)
            {
                policyName += $"[profileId:{profileId}]";
            }
            return base.AuthorizeAsync(user, resource, policyName);
        }
    }
}

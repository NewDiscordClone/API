using Application.Models;
using Microsoft.AspNetCore.Authorization;

namespace Application.Interfaces.Authorization
{
    public interface IServerAuthorizationService
    {
        Task<AuthorizationResult> AuthorizeAsync(
            ServerProfile server,
            object? resource,
            IEnumerable<IServerAuthorizationRequirement> requirements);
        Task<AuthorizationResult> AuthorizeAsync(ServerProfile profile, object? resource, string policyName);
    }
}

using Application.Models;

namespace Application.Providers
{
    public interface IAuthorizedUserProvider
    {
        int GetUserId();
        bool IsInRole(Role role, Server server);
        bool IsInRole(string roleName, int serverId);
        Task<bool> IsInRoleAsync(string roleName, int serverId);
    }
}
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IRoleFactory
    {
        Task<List<Role>> GetDefaultServerRolesAsync(string serverId);
        List<Role> GetGroupChatRoles(string chatId);
        Role GetGroupChatMemberRole(string chatId);
        Role GetGroupChatOwnerRole(string chatId);
        Role GetRoleForPersonalChat(string chatId);
    }
}

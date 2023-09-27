using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IRoleFactory
    {
        Role PersonalChatMemberRole { get; }
        Role GroupChatMemberRole { get; }
        Role GroupChatOwnerRole { get; }
        string[] GroupChatOwnerClaims { get; }
        string[] GroupChatMemberClaims { get; }
        string[] PersonalChatMemberClaims { get; }

        Task<List<Role>> GetDefaultServerRolesAsync(string serverId);
        List<Role> GetGroupChatRoles();
    }
}

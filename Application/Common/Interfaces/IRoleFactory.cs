using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IRoleFactory
    {
        Role PersonalChatMemberRole { get; }
        Role GroupChatMemberRole { get; }
        Role GroupChatOwnerRole { get; }
        Role ServerOwnerRole { get; }
        Role ServerMemberRole { get; }
        string[] GroupChatOwnerClaims { get; }
        string[] GroupChatMemberClaims { get; }
        string[] PersonalChatMemberClaims { get; }
        string[] ServerMemberDefaultClaims { get; }
        string[] ServerOwnerDefaultClaims { get; }

        Task<Role> CreateRoleAsync(string name, string color, int priority, string[] claims, string? serverId);
        List<Role> GetDefaultServerRoles();
        List<Role> GetGroupChatRoles();
    }
}

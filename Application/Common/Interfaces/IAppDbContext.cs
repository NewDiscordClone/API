using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces;

public interface IAppDbContext
{
    ISimpleDbSet<UserConnections> UserConnections { get; }
    ISimpleDbSet<Message> Messages { get; }
    ISimpleDbSet<Chat> Chats { get; }
    ISimpleDbSet<PersonalChat> PersonalChats { get; }
    ISimpleDbSet<GroupChat> GroupChats { get; }
    ISimpleDbSet<Channel> Channels { get; }
    ISimpleDbSet<Models.Media> Media { get; }
    ISimpleDbSet<Server> Servers { get; }
    ISimpleDbSet<Invitation> Invitations { get; }
    ISimpleDbSet<RelationshipList> RelationshipLists { get; }
    ISimpleDbSet<UserProfile> UserProfiles { get; }

    ISimpleDbSet<Role> SqlRoles { get; }

    ISimpleDbSet<User> SqlUsers { get; }
    // DbSet<Role> Roles { get; set; }
    // DbSet<User> Users { get; set; }
    Task SaveChangesAsync();

    void SetToken(CancellationToken cancellationToken);
    Task CheckRemoveMedia(string id);
    Task<List<Message>> GetMessagesAsync(string chatId, int skip, int take);
    Task<List<Message>> GetPinnedMessagesAsync(string chatId);
    Task<List<IdentityRoleClaim<Guid>>> GetRoleClaimAsync(Role role);
    Task AddClaimToRoleAsync(Role role, IdentityRoleClaim<Guid> claim);
    Task AddClaimsToRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims);
    Task RemoveClaimsFromRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims);
    Task RemoveClaimsFromRoleAsync(Role role);
    Task RemoveClaimFromRoleAsync(Role role, IdentityRoleClaim<Guid> claim);


}
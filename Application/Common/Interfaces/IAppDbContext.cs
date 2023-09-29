using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces;

public interface IAppDbContext
{
    ISimpleDbSet<UserConnections, Guid> UserConnections { get; }
    ISimpleDbSet<Message, string> Messages { get; }
    ISimpleDbSet<Chat, string> Chats { get; }
    ISimpleDbSet<PersonalChat, string> PersonalChats { get; }
    ISimpleDbSet<GroupChat, string> GroupChats { get; }
    ISimpleDbSet<Channel, string> Channels { get; }
    ISimpleDbSet<Media, string> Media { get; }
    ISimpleDbSet<Server, string> Servers { get; }
    ISimpleDbSet<Invitation, string> Invitations { get; }
    ISimpleDbSet<RelationshipList, Guid> RelationshipLists { get; }
    ISimpleDbSet<Role, Guid> SqlRoles { get; }

    ISimpleDbSet<User, Guid> SqlUsers { get; }
    DbSet<Role> Roles { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserProfile> UserProfiles { get; set; }
    Task SaveChangesAsync();

    void SetToken(CancellationToken cancellationToken);
    Task CheckRemoveMedia(string id);
    Task<List<Message>> GetMessagesAsync(string chatId, int skip, int take);
    Task<List<Message>> GetPinnedMessagesAsync(string chatId);
}
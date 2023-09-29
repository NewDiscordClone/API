using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces;

public interface IAppDbContext
{
    IRepository<UserConnections, Guid> UserConnections { get; }
    IRepository<Message, string> Messages { get; }
    IRepository<Chat, string> Chats { get; }
    IRepository<PersonalChat, string> PersonalChats { get; }
    IRepository<GroupChat, string> GroupChats { get; }
    IRepository<Channel, string> Channels { get; }
    IRepository<Media, string> Media { get; }
    IRepository<Server, string> Servers { get; }
    IRepository<Invitation, string> Invitations { get; }
    IRepository<Role, Guid> SqlRoles { get; }

    IRepository<User, Guid> SqlUsers { get; }
    DbSet<Role> Roles { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserProfile> UserProfiles { get; set; }
    Task SaveChangesAsync();

    void SetToken(CancellationToken cancellationToken);
    Task CheckRemoveMedia(string id);
    Task<List<Message>> GetMessagesAsync(string chatId, int skip, int take);
    Task<List<Message>> GetPinnedMessagesAsync(string chatId);
}
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IAppDbContext
{
    DbSet<Attachment> Attachments { get; set; }
    DbSet<Channel> Channels { get; set; }
    DbSet<Chat> Chats { get; set; }
    DbSet<Message> Messages { get; set; }
    DbSet<PrivateChat> PrivateChats { get; set; }
    DbSet<Reaction> Reactions { get; set; }
    DbSet<Role> Roles { get; set; }
    DbSet<Server> Servers { get; set; }
    DbSet<ServerProfile> ServerProfiles { get; set; }
    DbSet<User> Users { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

}
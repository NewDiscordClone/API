using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace Application.Interfaces;

public interface IAppDbContext
{
    public DbSet<Attachment> Attachments { get; set; } 
    public DbSet<Channel> Channels { get; set; }
    public DbSet<Chat> Chats { get; set; } 
    public DbSet<Message> Messages { get; set; }
    public DbSet<PrivateChat> PrivateChats { get; set; } 
    public DbSet<Reaction> Reactions { get; set; } 
    public DbSet<Role> Roles { get; set; } 
    public DbSet<Server> Servers { get; set; }
    public DbSet<ServerProfile> ServerProfiles { get; set; }
    public DbSet<User> Users { get; set; } 
}
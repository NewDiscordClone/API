using Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace DataAccess;

public class AppDbContext : DbContext, IAppDbContext
{
    private readonly IConfiguration _configuration;
    
    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Auth"));
    }

    public DbSet<Attachment> Attachments { get; set; } = null!;
    public DbSet<Channel> Channels { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public DbSet<Message> Messages { get; set; } = null!;
    public DbSet<PrivateChat> PrivateChats { get; set; } = null!;
    public DbSet<Reaction> Reactions { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Server> Servers { get; set; } = null!;
    public DbSet<ServerProfile> ServerProfiles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}
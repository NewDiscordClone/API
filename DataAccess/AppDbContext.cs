using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class AppDbContext : IdentityDbContext<User, Role, int>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Attachment> Attachments { get; set; } = null!;
        public DbSet<Channel> Channels { get; set; } = null!;
        public DbSet<Chat> Chats { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<PrivateChat> PrivateChats { get; set; } = null!;
        public DbSet<Reaction> Reactions { get; set; } = null!;
        public DbSet<Server> Servers { get; set; } = null!;
        public DbSet<ServerProfile> ServerProfiles { get; set; } = null!;

        public async Task<TResult> FindByIdAsync<TResult>(int id, CancellationToken cancellationToken = default)
            where TResult : class
        {
            DbSet<TResult> dbset = Set<TResult>();
            return await dbset.FindAsync(new object[] { id }, cancellationToken)
                ?? throw new EntityNotFoundException($"{typeof(TResult).Name} {id} not found");
        }
    }
}
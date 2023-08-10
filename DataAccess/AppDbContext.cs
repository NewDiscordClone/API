using System.Linq.Expressions;
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

        public async Task<TEntity> FindByIdAsync<TEntity>(int id, CancellationToken cancellationToken = default,
            params string[] includedProperties) where TEntity : class
        {
            DbSet<TEntity> dbSet = Set<TEntity>();
            IQueryable<TEntity> queryable = dbSet.AsQueryable();

            foreach (var property in includedProperties)
            {
                queryable = queryable.Include(property);
            }

            TEntity? entity = await queryable
                .FirstOrDefaultAsync(e => GetId(e) == id, cancellationToken);

            if (entity == null)
            {
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found");
            }

            return entity;
        }

        private int GetId<TEntity>(TEntity entity)
        {
            var idProperty = typeof(TEntity).GetProperty("Id");
    
            if (idProperty == null)
            {
                throw new InvalidOperationException($"Type {typeof(TEntity).Name} does not have an 'Id' property.");
            }
    
            return (int)idProperty.GetValue(entity);
        }
    }
}
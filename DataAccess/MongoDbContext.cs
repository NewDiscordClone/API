using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess
{
    public class MongoDbContext(DbContextOptions<MongoDbContext> options) : DbContext(options)
    {
        public DbSet<Server> Servers { get; set; }
        public DbSet<UserConnections> UserConnections { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public static MongoDbContext Create(IMongoDatabase database)
        {
            return new(new DbContextOptionsBuilder<MongoDbContext>()
               .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
               .Options);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Server>().ToCollection("servers");
            modelBuilder.Entity<UserConnections>().ToCollection("userConnections");
            modelBuilder.Entity<Message>().ToCollection("messages");
            modelBuilder.Entity<Chat>().ToCollection("chats");
            modelBuilder.Entity<Media>().ToCollection("media");
            modelBuilder.Entity<Invitation>().ToCollection("invitations");
        }
    }
}

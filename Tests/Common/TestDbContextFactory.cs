namespace Tests.Common
{
    using DataAccess;
    using global::Application.Interfaces;
    using global::Application.Models;
    using Microsoft.EntityFrameworkCore;

    namespace Application.Tests.Common
    {
        public static class TestDbContextFactory
        {
            public static int UserAId { get; set; } = 1;
            public static int UserBId { get; set; } = 2;

            public static int ServerIdForDelete { get; set; } = 1;
            public static int ServerIdForUpdate { get; set; } = 2;

            public static IAppDbContext Create()
            {
                DbContextOptions<AppDbContext> options =
                    new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
                AppDbContext context = new(options);
                context.Database.EnsureCreated();

                context.Servers.AddRange(
                     new Server
                     {
                         Id = ServerIdForDelete,
                         Title = "Server 1",
                         ServerProfiles = new List<ServerProfile>
                    {
                        new ServerProfile
                        {
                            User = new User { Id = UserAId }
                        }
                    }
                     },
                new Server
                {
                    Id = ServerIdForUpdate,
                    Title = "Server 2",
                    ServerProfiles = new List<ServerProfile>
                    {
                        new ServerProfile
                        {
                            User = new User { Id = UserBId }
                        }
                    }
                });
                context.SaveChanges();
                return context;
            }

            public static void Destroy(AppDbContext context)
            {
                context.Database.EnsureDeleted();
                context.Dispose();
            }
        }
    }

}

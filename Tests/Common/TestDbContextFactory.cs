using Application.Common.Mapping;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Tests.Common
{
    public static class TestDbContextFactory
    {
        private static IMongoClient _mongoClient;

        public static IAppDbContext Create(out Ids ids)
        {
            ids = new Ids();
            DbContextOptions<AppDbContext> options =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            _mongoClient = new MongoClient("mongodb://localhost:27017");
            AppDbContext context = new(options, _mongoClient, Guid.NewGuid().ToString());
            context.Database.EnsureCreated();
            IMapper mapper = new MapperConfiguration(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly))).CreateMapper();

            HardCodedData hardCodedData = new(ids, mapper);
            
            // Role ownerRole = new()
            // {
            //     Name = "Owner",
            //     Color = "#FFFF00"
            // };

            context.Users.AddRange(hardCodedData.Users);
            context.Servers.AddMany(hardCodedData.Servers);
            context.Channels.AddMany(hardCodedData.Channels);
            context.GroupChats.AddMany(hardCodedData.GroupChats);
            context.PersonalChats.AddMany(hardCodedData.PersonalChats);
            context.Messages.AddMany(hardCodedData.Messages);
            context.Invitations.AddMany(hardCodedData.Invitations);
            context.RelationshipLists.AddMany(hardCodedData.Relationships);

            context.SaveChanges();
            return context;
        }

        public static IAppDbContext CreateFake(out Ids ids)
        {
            ids = new Ids();
            DbContextOptions<FakeDbContext> options =
                new DbContextOptionsBuilder<FakeDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            IMapper mapper = new MapperConfiguration(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly))).CreateMapper();
            FakeDbContext context = new(options, mapper);
            HardCodedData hardCodedData = new(ids, mapper);
            context.Create(hardCodedData);
            return context;
        }

        public static void Destroy(IAppDbContext iContext)
        {
            if (iContext is not AppDbContext context)
                return;
            context.Database.EnsureDeleted();
            context.Dispose();
        }
    }
}
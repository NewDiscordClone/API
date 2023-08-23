using System.Linq.Expressions;
using Application.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Interfaces;

public interface IAppDbContext
{
    IMongoCollection<Message> Messages { get; }
    IMongoCollection<Chat> Chats { get; }
    IMongoCollection<PrivateChat> PrivateChats { get; }
    IMongoCollection<Channel> Channels { get; }
    DbSet<Role> Roles { get; set; }
    DbSet<Server> Servers { get; }
    DbSet<ServerProfile> ServerProfiles { get; }
    DbSet<User> Users { get; set; }
    IMongoDatabase MongoDb { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<TResult> FindSqlByIdAsync<TResult>(int id, CancellationToken cancellationToken = default, params string[] includedProperties) where TResult : class;
    FilterDefinition<TEntity> GetIdFilter<TEntity>(ObjectId id);
    Task<TResult> FindByIdAsync<TResult>(ObjectId id, CancellationToken cancellationToken = default) where TResult : class;

}
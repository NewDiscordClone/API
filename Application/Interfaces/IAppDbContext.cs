using System.Linq.Expressions;
using Application.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
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
    IMongoCollection<Media> Media { get; }
    //DbSet<Role> Roles { get; set; }
    IMongoCollection<Server> Servers { get; }
    //DbSet<ServerProfile> ServerProfiles { get; }
    IMongoCollection<User> Users { get;}
    IMongoDatabase MongoDb { get; }

    Task CheckRemoveMedia(string id, CancellationToken cancellationToken = default);
    FilterDefinition<TEntity> GetIdFilter<TEntity>(ObjectId id);
    Task<TResult> FindByIdAsync<TResult>(ObjectId id, CancellationToken cancellationToken = default) where TResult : class;

}
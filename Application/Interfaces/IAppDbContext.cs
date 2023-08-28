using System.Linq.Expressions;
using Application.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Interfaces;

public interface IAppDbContext
{
    ISimpleDbSet<Message> Messages { get; }
    ISimpleDbSet<Chat> Chats { get; }
    ISimpleDbSet<PrivateChat> PrivateChats { get; }
    ISimpleDbSet<Channel> Channels { get; }
    ISimpleDbSet<Media> Media { get; }
    ISimpleDbSet<Server> Servers { get; }
    //DbSet<ServerProfile> ServerProfiles { get; }
    DbSet<Role> Roles { get; set; }
    DbSet<User> Users { get; set; }
    IMongoDatabase MongoDb { get; }

    void SetToken(CancellationToken cancellationToken);
    Task CheckRemoveMedia(string id);
    Task<TResult> FindSqlByIdAsync<TResult>(int id, CancellationToken cancellationToken = default, params string[] includedProperties) where TResult : class;

}
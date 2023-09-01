using Application.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Application.Interfaces;

public interface IAppDbContext
{
    ISimpleDbSet<UserConnections> UserConnections { get; }
    ISimpleDbSet<Message> Messages { get; }
    ISimpleDbSet<Chat> Chats { get; }
    ISimpleDbSet<PersonalChat> PersonalChats { get; }
    ISimpleDbSet<GroupChat> GroupChats { get; }
    ISimpleDbSet<Channel> Channels { get; }
    ISimpleDbSet<Media> Media { get; }
    ISimpleDbSet<Server> Servers { get; }
    //DbSet<ServerProfile> ServerProfiles { get; }
    DbSet<Role> Roles { get; set; }
    DbSet<User> Users { get; set; }
    Task SaveChangesAsync();

    void SetToken(CancellationToken cancellationToken);
    Task CheckRemoveMedia(string id);
    Task<TResult> FindSqlByIdAsync<TResult>(int id, CancellationToken cancellationToken = default, params string[] includedProperties) where TResult : class;
    Task<List<Message>> GetMessagesAsync(string chatId, int skip, int take);
    Task<List<Message>> GetPinnedMessagesAsync(string chatId);
    Task<List<Claim>> GetRoleClaimAsync(Role role);
    Task AddClaimToRoleAsync(Role role, Claim claim);
    Task AddClaimsToRoleAsync(Role role, IEnumerable<Claim> claims);


}
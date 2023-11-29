using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class ConnectionsRepository(MongoDbContext context, IUserProfileRepository profileRepository)
                : Repository<MongoDbContext, UserConnections, Guid>(context), IConnectionsRepository
    {
        private readonly IUserProfileRepository _profileRepository = profileRepository;

        public async Task<IEnumerable<string>> FindConnectionsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(connection => ids.Contains(connection.UserId))
                .SelectMany(connection => connection.Connections)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<string>> FindConnectionsAsync(CancellationToken cancellationToken = default, params Guid[] ids)
            => await FindConnectionsAsync(ids, cancellationToken);

        public async Task<IEnumerable<string>> FindConnectionsAsync(Guid id, CancellationToken cancellationToken = default)
            => await FindConnectionsAsync([id], cancellationToken);


        public async Task<IEnumerable<string>> FindConnectionsAsync(Server server, CancellationToken cancellationToken = default)
        {
            Guid[] userIds = await _profileRepository.ExecuteCustomQuery(profiles => profiles
                 .OfType<ServerProfile>()
               .Where(profile => profile.ServerId == server.Id)
               .Select(profile => profile.UserId))
               .ToArrayAsync(cancellationToken);

            return await FindConnectionsAsync(userIds, cancellationToken);
        }

        public async Task<IEnumerable<string>> FindConnectionsAsync(Chat chat, CancellationToken cancellationToken = default)
        {
            Guid[] userIds = chat switch
            {
                PersonalChat p => await GetPersonalChatUserIdsAsync(p, cancellationToken),
                Channel c => await GetChannelUserIdsAsync(c, cancellationToken),
                _ => throw new Exception("Unknown chat type")
            };

            return await FindConnectionsAsync(userIds, cancellationToken);
        }

        private async Task<Guid[]> GetChannelUserIdsAsync(Channel cannel, CancellationToken cancellationToken = default)
        {
            return await _profileRepository.ExecuteCustomQuery(profiles => profiles
                  .OfType<ServerProfile>()
                  .Where(profile => profile.ServerId == cannel.ServerId)
                  .Select(profile => profile.UserId))
                  .ToArrayAsync(cancellationToken);
        }

        private async Task<Guid[]> GetPersonalChatUserIdsAsync(PersonalChat chat, CancellationToken cancellationToken = default)
        {
            return await _profileRepository.ExecuteCustomQuery(profiles => profiles
                  .Where(profile => profile.ChatId == chat.Id)
                  .Select(profile => profile.UserId))
                  .ToArrayAsync(cancellationToken);
        }
    }
}
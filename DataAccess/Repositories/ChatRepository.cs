using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class ChatRepository(MongoDbContext context) :
        Repository<MongoDbContext, Chat, string>(context), IChatRepository
    {
        public IQueryable<Channel> Channels => DbSet.OfType<Channel>();

        public IQueryable<GroupChat> GroupChats => DbSet.OfType<GroupChat>();

        public async Task<TChat> FindAsync<TChat>(string id, CancellationToken cancellationToken = default)
            where TChat : Chat
        {
            Chat chat = await FindAsync(id, cancellationToken);

            if (chat is not TChat tChat)
                throw new InvalidOperationException($"Chat {id} is not {nameof(TChat)}");

            return tChat;
        }
    }
}

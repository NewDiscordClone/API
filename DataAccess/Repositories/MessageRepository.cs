using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class MessageRepository(MongoDbContext context)
        : Repository<MongoDbContext, Message, string>(context), IMessageRepository
    {
        public async Task<IEnumerable<Message>> GetMessagesInChatAsync(string chatId, int skip, int take,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .Where(message => message.ChatId == chatId)
                .Skip(skip).Take(take)
                .ToListAsync(cancellationToken);
        }
    }
}

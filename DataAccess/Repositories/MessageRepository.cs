namespace Sparkle.DataAccess.Repositories
{
    public class MessageRepository
    {
        //public async Task<List<Message>> GetMessagesAsync(
        // string chatId,
        // int skip,
        // int take)
        // => await MongoDb.GetCollection<Message>("messages")
        //     .Find(Builders<Message>.Filter.Eq("ChatId", chatId))
        //     .SortByDescending(m => m.SendTime)
        //.Skip(skip)
        //     .Limit(take)
        //     .ToListAsync(_token);

        //public async Task<List<Message>> GetPinnedMessagesAsync(
        //    string chatId
        //) => await MongoDb.GetCollection<Message>("messages")
        //    .Find(
        //        Builders<Message>.Filter.Eq("ChatId", chatId) &
        //        Builders<Message>.Filter.Eq("IsPinned", true)
        //    )
        //    .SortByDescending(m => m.PinnedTime)
        //    .ToListAsync(_token);
    }
}

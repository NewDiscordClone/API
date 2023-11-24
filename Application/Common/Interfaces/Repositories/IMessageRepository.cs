﻿using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IMessageRepository : IRepository<Message, string>
    {
        Task<IEnumerable<Message>> GetMessagesInChatAsync(string chatId, int messagesCount, int pageSize);
    }
}
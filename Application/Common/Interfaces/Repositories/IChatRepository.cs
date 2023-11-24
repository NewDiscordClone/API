using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IChatRepository : IRepository<Chat, string>
    {
        IQueryable<Channel> Channels { get; }
        IQueryable<GroupChat> GroupChats { get; }


        Task<TChat> FindAsync<TChat>(string id, CancellationToken cancellationToken = default) where TChat : Chat;
    }
}
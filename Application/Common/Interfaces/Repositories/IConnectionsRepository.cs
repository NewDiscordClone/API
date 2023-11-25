using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IConnectionsRepository : IRepository<UserConnections, Guid>
    {
        Task<IEnumerable<string>> FindAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> FindAsync(CancellationToken cancellationToken = default, params Guid[] ids);
        new Task<IEnumerable<string>> FindAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> FindAsync(Server server, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> FindAsync(Chat chat, CancellationToken cancellationToken = default);
    }
}

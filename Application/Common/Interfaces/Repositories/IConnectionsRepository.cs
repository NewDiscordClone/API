using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IConnectionsRepository : IRepository<UserConnections, Guid>
    {
        Task<IEnumerable<string>> FindConnectionsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> FindConnectionsAsync(CancellationToken cancellationToken = default, params Guid[] ids);
        Task<IEnumerable<string>> FindConnectionsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> FindConnectionsAsync(Server server, CancellationToken cancellationToken = default);
        Task<IEnumerable<string>> FindConnectionsAsync(Chat chat, CancellationToken cancellationToken = default);
    }
}

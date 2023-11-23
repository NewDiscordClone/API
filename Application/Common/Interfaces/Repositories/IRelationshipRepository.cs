using Sparkle.Application.Models;
using RelationshipId = (System.Guid Active, System.Guid Passive);

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    /// <summary>
    /// Represents a repository for managing relationships between two users.
    /// </summary>
    public interface IRelationshipRepository : IRepository<Relationship, RelationshipId>
    {
        Task<Relationship> FindByChatIdAsync(string chatId, CancellationToken cancellationToken);

        /// <summary>
        /// Updates the relationship with ability to swap keys .
        /// </summary>
        /// <param name="relationship">Relationship to update</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Updated relationship</returns>
        public Task<Relationship> UpdateWithKeysAsync(Relationship relationship, CancellationToken cancellationToken = default);
    }
}

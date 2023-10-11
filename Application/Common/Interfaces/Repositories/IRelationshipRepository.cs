using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    /// <summary>
    /// Represents a repository for managing relationships between two users.
    /// </summary>
    public interface IRelationshipRepository : IRepository<Relationship, (Guid Active, Guid Passive)>
    {
        /// <summary>
        /// Updates the relationship with ability to swap keys .
        /// </summary>
        /// <param name="relationship">Relationship to update</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Updated relationship</returns>
        public Task<Relationship> UpdateWithKeysAsync(Relationship relationship, CancellationToken cancellationToken = default);
    }
}

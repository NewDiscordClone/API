using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    /// <summary>
    /// Represents a repository for managing relationships between two users.
    /// </summary>
    public interface IRelationshipRepository : IRepository<Relationship, (Guid Active, Guid Passive)>
    {
    }
}

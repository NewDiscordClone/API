using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IRelationshipRepository : IRepository<Relationship, (Guid Active, Guid Passive)>
    {
    }
}

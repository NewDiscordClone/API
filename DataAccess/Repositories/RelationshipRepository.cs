using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="Relationship"/> entities.
    /// </summary>
    public class RelationshipRepository : BaseSqlRepository<Relationship, (Guid Active, Guid Passive)>, IRelationshipRepository
    {
        public RelationshipRepository(AppDbContext context) : base(context)
        {
        }

        public override void AddMany(IEnumerable<Relationship> entities)
        {
            throw new InvalidOperationException($"{nameof(AddMany)} method forbidden for " +
                $"{nameof(Relationship)}s. Use {nameof(AddAsync)} instead");
        }

        public override Task AddManyAsync(IEnumerable<Relationship> entities, CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException($"{nameof(AddMany)} method forbidden for " +
             $"{nameof(Relationship)}s. Use {nameof(AddAsync)} instead");
        }

        public override async Task DeleteAsync((Guid Active, Guid Passive) id, CancellationToken cancellationToken = default)
        {
            Relationship relationship = await FindAsync(id, cancellationToken);

            await DeleteAsync(relationship, cancellationToken);
        }

        public override async Task<Relationship> AddAsync(Relationship entity, CancellationToken cancellationToken = default)
        {
            {
                Relationship? reverseRelationship = await
                    FindOrDefaultAsync((entity.Passive, entity.Active), cancellationToken);

                if (reverseRelationship != null)
                {
                    reverseRelationship.RelationshipType = entity.RelationshipType;
                    DbSet.Entry(reverseRelationship).State = EntityState.Modified;
                    await Context.SaveChangesAsync(cancellationToken);

                    return reverseRelationship;
                }
                else
                {
                    Relationship addedRelationship = await base.AddAsync(entity, cancellationToken);
                    return addedRelationship;
                }
            }
        }

        public override async Task<Relationship> FindAsync((Guid Active, Guid Passive) id, CancellationToken cancellationToken = default)
        {
            return await FindOrDefaultAsync(id, cancellationToken)
                ?? throw new EntityNotFoundException("Relationship not found", id);
        }

        /// <summary>
        /// Finds a <see cref="Relationship"/> entity with the specified IDs, or returns null if not found.
        /// </summary>
        /// <param name="id">The IDs of the relationship to find.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The found relationship, or null if not found.</returns>
        public async Task<Relationship?> FindOrDefaultAsync((Guid Active, Guid Passive) id,
            CancellationToken cancellationToken = default)
        {
            Relationship? relationship = await DbSet.FindAsync(new object?[] { id.Active, id.Passive }, cancellationToken: cancellationToken);
            return relationship ?? await DbSet.FindAsync(new object?[] { id.Passive, id.Active }, cancellationToken: cancellationToken);
        }

    }
}

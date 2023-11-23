using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;
using RelationshipId = (System.Guid Active, System.Guid Passive);

namespace Sparkle.DataAccess.Repositories
{
    /// <summary>
    /// Repository for managing <see cref="Relationship"/> entities.
    /// </summary>
    public class RelationshipRepository(PostgresDbContext context)
        : BaseRepository<PostgresDbContext, Relationship, RelationshipId>(context), IRelationshipRepository
    {
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

        public override async Task DeleteAsync(RelationshipId id, CancellationToken cancellationToken = default)
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
                    throw new InvalidOperationException("Relationship already exists");

                Relationship addedRelationship = await base.AddAsync(entity, cancellationToken);
                return addedRelationship;
            }
        }

        public override async Task<Relationship> FindAsync(RelationshipId id, CancellationToken cancellationToken = default)
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
        public async Task<Relationship?> FindOrDefaultAsync(RelationshipId id,
            CancellationToken cancellationToken = default)
        {
            Relationship? relationship = await DbSet.FindAsync([id.Active, id.Passive], cancellationToken: cancellationToken);
            return relationship ?? await DbSet.FindAsync([id.Passive, id.Active], cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Updates a relationship with the given keys. If the relationship already exists with swapped keys, it will deleted and added.
        /// Otherwise, it will be updated.
        /// </summary>
        /// <remarks> This method is used to update a relationship with the ability to swap keys.</remarks>
        /// <param name="relationship">The relationship to update.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated relationship.</returns>
        public async Task<Relationship> UpdateWithKeysAsync(Relationship relationship, CancellationToken cancellationToken = default)

        {
            Relationship originalRelationship = await FindAsync((relationship.Active, relationship.Passive),
                cancellationToken);

            if (originalRelationship == relationship)
            {
                Context.Entry(originalRelationship).State = EntityState.Detached;
                Context.Entry(relationship).State = EntityState.Detached;
                await UpdateAsync(relationship, cancellationToken);
            }
            else
            {
                await DeleteAsync((relationship.Active, relationship.Passive), cancellationToken);
                await AddAsync(relationship, cancellationToken);
            }

            return relationship;
        }

        public async Task<Relationship> FindByChatIdAsync(string chatId, CancellationToken cancellationToken)
            => await DbSet.SingleAsync(relationship => relationship.PersonalChatId == chatId, cancellationToken);
    }
}

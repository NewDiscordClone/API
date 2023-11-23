using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using System.Linq.Expressions;

namespace Sparkle.Application.Common.Interfaces
{

    /// <summary>
    /// Represents a generic repository interface for performing CRUD operations on entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity that the repository operates on.</typeparam>
    /// <typeparam name="TId">The type of the entity's identifier.</typeparam>
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        /// <summary>
        /// Finds an entity with the specified identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to find.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified identifier, or null if the entity was not found.</returns>
        /// <exception cref="EntityNotFoundException">Thrown when the entity with the specified identifier was not found.</exception>
        Task<TEntity> FindAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds an entity with the specified identifier, or returns null if the entity was not found.
        /// </summary>
        /// <param name="id">The identifier of the entity to find.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity with the specified identifier, or null if the entity was not found.</returns>
        async Task<TEntity?> FindOrDefaultAsync(TId id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await FindAsync(id, cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Filters the entities based on a predicate.
        /// </summary>
        /// <param name="query">The predicate used to filter the entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that satisfy the predicate.</returns>
        List<TEntity> ExecuteCustomQuery(Func<DbSet<TEntity>, IQueryable<TEntity>> query);

        /// <summary>
        /// Filters the entities based on a predicate.
        /// </summary>
        /// <param name="query">The predicate used to filter the entities.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a list of entities that satisfy the predicate.</returns>
        Task<List<TEntity>> ExecuteCustomQueryAsync(Func<DbSet<TEntity>, IQueryable<TEntity>> query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finds a single entity that satisfies a predicate.
        /// </summary>
        /// <param name="expression">The predicate used to find the entity.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity that satisfies the predicate.</returns>
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        void AddMany(IEnumerable<TEntity> entities);

        /// <summary>
        /// Adds an entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds multiple entities to the repository.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated entity.</returns>
        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes an entity with the specified identifier from the repository.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes multiple entities from the repository based on a predicate.
        /// </summary>
        /// <param name="query">The predicate used to filter the entities to delete.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteManyAsync(Func<DbSet<TEntity>, IQueryable<TEntity>> query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes multiple entities from the repository.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Counts the number of entities that satisfy a predicate.
        /// </summary>
        /// <param name="expression">The predicate used to count the entities.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the number of entities that satisfy the predicate.</returns>
        Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    }
}
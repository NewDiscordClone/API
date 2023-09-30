using System.Linq.Expressions;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        Task<TEntity> FindAsync(TId id, CancellationToken cancellationToken = default);

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

        Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);

        void AddMany(IEnumerable<TEntity> entities);
        Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task AddManyAsync(IEnumerable<TEntity> profiles, CancellationToken cancellationToken = default);

        Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
        Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default);
    }
}
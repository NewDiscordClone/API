using System.Linq.Expressions;

namespace Sparkle.Application.Common.Interfaces
{
    public interface ISimpleDbSet<TEntity, TId> where TEntity : class
    {
        CancellationToken CancellationToken { set; }

        Task<TEntity> FindAsync(TId id);

        async Task<TEntity?> FindOrDefaultAsync(TId id)
        {
            try
            {
                return await FindAsync(id);
            }
            catch
            {
                return null;
            }
        }

        Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression);

        void AddMany(IEnumerable<TEntity> entities);
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TId id);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> expression);
        Task AddManyAsync(IEnumerable<TEntity> profiles);
    }
}
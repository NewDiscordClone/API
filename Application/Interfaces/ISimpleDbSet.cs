using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Interfaces
{
    public interface ISimpleDbSet<TEntity> where TEntity : class
    {
        CancellationToken CancellationToken { set; }
         
        Task<TEntity> FindAsync(object id);
        
        Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression);

        void AddMany(IEnumerable<TEntity> entities);
        Task<TEntity> AddAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression);

        Task<long> CountAsync(Expression<Func<TEntity, bool>> expression);
    }
}
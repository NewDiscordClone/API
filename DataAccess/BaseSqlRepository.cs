using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace Sparkle.DataAccess
{
    /// <summary>
    /// Base repository class for SQL data access.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The entity key type.</typeparam>
    public class BaseSqlRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        public DbSet<TEntity> DbSet => _dbSet;
        public PostgresDbContext Context => _context;


        private readonly DbSet<TEntity> _dbSet;
        private readonly PostgresDbContext _context;

        public BaseSqlRepository(DbSet<TEntity> original, PostgresDbContext context)
        {
            _dbSet = original;
            _context = context;
        }

        public BaseSqlRepository(PostgresDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public virtual async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object?[] { id }, cancellationToken: cancellationToken) ??
                   throw new EntityNotFoundException($"Entity {typeof(TEntity).Name} ({id}) not found", id!);
        }

        public virtual async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            List<TEntity> res = await _dbSet.Where(expression).ToListAsync(cancellationToken);
            return res;
        }

        public virtual void AddMany(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            _context.SaveChanges();
        }

        public virtual async Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<TEntity> result = await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<TEntity> result = _dbSet.Update(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(await FindAsync(id, cancellationToken));
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(await FilterAsync(expression, cancellationToken));
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(entities);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _dbSet.SingleAsync(expression, cancellationToken);
        }
    }
}
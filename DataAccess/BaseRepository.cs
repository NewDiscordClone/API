using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace Sparkle.DataAccess
{
    /// <summary>
    /// Base repository class for data access.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being accessed.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public class BaseRepository<TContext, TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
        where TContext : DbContext
    {
        protected readonly DbSet<TEntity> DbSet;
        protected readonly TContext Context;

        public BaseRepository(TContext context)
        {
            Context = context;
            DbSet = Context.Set<TEntity>();
        }

        public virtual async Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return await DbSet.FindAsync([id], cancellationToken) ??
                   throw new EntityNotFoundException($"Entity {typeof(TEntity).Name} ({id}) not found", id!);
        }

        public async Task<TEntity?> FindOrDefaultAsync(TKey id, CancellationToken cancellationToken = default)
            => await DbSet.FindAsync([id], cancellationToken);

        public IQueryable<T> ExecuteCustomQuery<T>(Func<DbSet<TEntity>, IQueryable<T>> query)
        {
            return query(DbSet);
        }

        public virtual void AddMany(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            Context.SaveChanges();
        }

        public virtual async Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<TEntity> result = await DbSet.AddAsync(entity, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            EntityEntry<TEntity> result = DbSet.Update(entity);
            await Context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            DbSet.Remove(await FindAsync(id, cancellationToken));
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteManyAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(DbSet.Where(predicate));
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            DbSet.RemoveRange(entities);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await DbSet.CountAsync(cancellationToken);
        }

        public virtual async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await DbSet.SingleAsync(expression, cancellationToken);
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace Sparkle.DataAccess
{
    public class SimpleSqlDbSet<TEntity> : ISimpleDbSet<TEntity, Guid> where TEntity : class
    {
        public CancellationToken CancellationToken { get; set; }

        public DbSet<TEntity> DbSet => _dbSet;

        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext _context;

        public SimpleSqlDbSet(DbSet<TEntity> original, DbContext context, CancellationToken cancellationToken = default)
        {
            _dbSet = original;
            _context = context;
            CancellationToken = cancellationToken;
        }

        public SimpleSqlDbSet(DbContext context, CancellationToken cancellationToken)
        {
            _context = context;
            CancellationToken = cancellationToken;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<TEntity> FindAsync(Guid id)
        {
            return await FindByIdAsync(id);
        }

        public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.Where(expression).ToListAsync(CancellationToken);
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
            _context.SaveChanges();
        }

        public async Task AddManyAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities, CancellationToken);
            await _context.SaveChangesAsync(CancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> result = await _dbSet.AddAsync(entity, CancellationToken);
            await _context.SaveChangesAsync(CancellationToken);
            return result.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> result = _dbSet.Update(entity);
            await _context.SaveChangesAsync(CancellationToken);
            return result.Entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(CancellationToken);
        }

        public async Task DeleteAsync(Guid id)
        {
            _dbSet.Remove(await FindByIdAsync(id));
            await _context.SaveChangesAsync(CancellationToken);
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            _dbSet.RemoveRange(await FilterAsync(expression));
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _dbSet.CountAsync(CancellationToken);
        }

        private async Task<TEntity> FindByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentException("Id is null");
            return await _dbSet.FindAsync(new[] { id }, CancellationToken) ??
                   throw new EntityNotFoundException($"Entity {typeof(TEntity).Name} ({id}) not found", id.ToString());
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
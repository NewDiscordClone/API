using System.Linq.Expressions;
using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SimpleSqlDbSet<TEntity> : ISimpleDbSet<TEntity> where TEntity : class
    {
        public CancellationToken CancellationToken { get; set; }

        private readonly DbSet<TEntity> _original;
        private readonly DbContext _context;

        public SimpleSqlDbSet(DbSet<TEntity> original, DbContext context, CancellationToken cancellationToken = default)
        {
            _original = original;
            _context = context;
            CancellationToken = cancellationToken;
        }
        
        public async Task<TEntity> FindAsync(object id)
        {
            return await FindByIdAsync(id);
        }

        public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _original.Where(expression).ToListAsync(CancellationToken);
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            _original.AddRange(entities);
            _context.SaveChanges();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var result = await _original.AddAsync(entity, CancellationToken);
            await _context.SaveChangesAsync(CancellationToken);
            return result.Entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var result = _original.Update(entity);
            await _context.SaveChangesAsync(CancellationToken);
            return result.Entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _original.Remove(entity);
            await _context.SaveChangesAsync(CancellationToken);
        }

        public async Task DeleteAsync(object id)
        {
            _original.Remove(await FindByIdAsync(id));
            await _context.SaveChangesAsync(CancellationToken);
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            _original.RemoveRange(await FilterAsync(expression));
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _original.CountAsync(CancellationToken);
        }

        private async Task<TEntity> FindByIdAsync(object id)
        {
            if (id == null) throw new ArgumentException("Id is null");
            return await _original.FindAsync(new[] { id }, CancellationToken) ??
                   throw new EntityNotFoundException($"Entity {typeof(TEntity).Name} ({id}) not found", id.ToString());
        }
    }
}
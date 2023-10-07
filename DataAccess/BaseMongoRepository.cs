using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Sparkle.DataAccess
{
    /// <summary>
    /// Base repository class for MongoDB data access.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity being accessed.</typeparam>
    /// <typeparam name="TKey">The type of the entity's primary key.</typeparam>
    public class BaseMongoRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {

        private IMongoCollection<TEntity> _collection;

        public BaseMongoRepository(IMongoCollection<TEntity> collection, CancellationToken cancellationToken = default)
        {
            _collection = collection;
        }

        public virtual Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            return FindByIdAsync(id, cancellationToken);
        }

        public virtual Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(expression).ToListAsync(cancellationToken);
        }


        public virtual void AddMany(IEnumerable<TEntity> entities)
        {
            _collection.InsertMany(entities, null);
        }

        public virtual async Task AddManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _collection.InsertManyAsync(entities, cancellationToken: cancellationToken);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            PropertyInfo idProp = GetIdProperty();

            object? id = idProp.GetValue(entity);

            if (typeof(TKey) == typeof(string) && id == null)
            {
                id = ObjectId.GenerateNewId().ToString();
                idProp.SetValue(entity, id);
            }

            await _collection.InsertOneAsync(entity, null, cancellationToken);
            return await FindByIdAsync((TKey)id!, cancellationToken);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            TKey? id = GetId(entity);
            await _collection.ReplaceOneAsync(
                GetIdFilter(id),
                entity,
                cancellationToken: cancellationToken);
            return await FindByIdAsync(id, cancellationToken);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(GetIdFilter(id), null, cancellationToken);
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteManyAsync(expression, null, cancellationToken);
        }

        public virtual Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return _collection.CountDocumentsAsync(expression, null, cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            TKey? id = GetId(entity);
            await _collection.DeleteOneAsync(GetIdFilter(id), null, cancellationToken);
        }

        protected static TKey GetId(TEntity entity)
        {
            return (TKey)GetIdProperty().GetValue(entity)!;
        }

        protected static PropertyInfo GetIdProperty()
        {
            return typeof(TEntity)
                       .GetProperties()
                       .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(BsonIdAttribute))) ??
                   throw new ArgumentException(
                       $"There is no property that has [BsonId] attribute in {typeof(TEntity).Name}");
        }

        private static TKey ConvertToId(object id)
        {
            if (typeof(TKey) != typeof(string))
                return (TKey)id;

            ObjectId objectId;

            objectId = id switch
            {
                string strId => ObjectId.Parse(strId),
                ObjectId objId => objId,
                _ => throw new ArgumentException("Id is not a string or ObjectId instance")
            };

            return (TKey)(objectId as object);
        }

        private static FilterDefinition<TEntity> GetIdFilter(TKey id)
        {
            object objectId = id!;
            if (typeof(TKey) == typeof(string))
                objectId = ObjectId.Parse(id!.ToString());

            return Builders<TEntity>.Filter.Eq("_id", objectId);
        }


        private async Task<TEntity> FindByIdAsync(TKey id, CancellationToken cancellationToken = default)
        {
            FilterDefinition<TEntity> filter = GetIdFilter(id);
            TEntity result =
                await (await _collection.FindAsync(filter, null, cancellationToken)).FirstOrDefaultAsync(
                    cancellationToken);

            return result
                ?? throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found", id.ToString());
        }

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteManyAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
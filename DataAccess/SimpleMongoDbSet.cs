using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace Sparkle.DataAccess
{
    public class SimpleMongoDbSet<TEntity, TKey> : ISimpleDbSet<TEntity, TKey> where TEntity : class
    {

        private IMongoCollection<TEntity> _collection;

        public SimpleMongoDbSet(IMongoCollection<TEntity> collection, CancellationToken cancellationToken = default)
        {
            _collection = collection;
        }

        public Task<TEntity> FindAsync(TKey id, CancellationToken cancellationToken = default)
        {
            ObjectId objectId = ConvertToId(id, cancellationToken);
            return FindByIdAsync(objectId, cancellationToken);
        }

        public Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return _collection.Find(expression).ToListAsync(cancellationToken);
        }


        public void AddMany(IEnumerable<TEntity> entities)
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
            if (id == null || !ObjectId.TryParse(id.ToString(), out ObjectId objectId))
            {
                objectId = ObjectId.GenerateNewId();
                idProp.SetValue(entity, objectId.ToString());
            }

            await _collection.InsertOneAsync(entity, null, cancellationToken);
            return await FindByIdAsync(objectId, cancellationToken);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ObjectId id = GetId(entity, cancellationToken);
            await _collection.ReplaceOneAsync(
                GetIdFilter(id, cancellationToken),
                entity,
                cancellationToken: cancellationToken);
            return await FindByIdAsync(id, cancellationToken);
        }

        public virtual async Task DeleteAsync(TKey id, CancellationToken cancellationToken = default)
        {
            ObjectId objId = ConvertToId(id, cancellationToken);
            await _collection.DeleteOneAsync(GetIdFilter(objId, cancellationToken), null, cancellationToken);
        }

        public virtual async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteManyAsync(expression, null, cancellationToken);
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> expression, CancellationToken cancellationToken = default)
        {
            return _collection.CountDocumentsAsync(expression, null, cancellationToken);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            ObjectId id = GetId(entity, cancellationToken);
            await _collection.DeleteOneAsync(GetIdFilter(id, cancellationToken), null, cancellationToken);
        }

        private static ObjectId GetId(TEntity entity, CancellationToken cancellationToken = default)
        {
            return ConvertToId(GetIdProperty().GetValue(entity), cancellationToken);
        }

        private static PropertyInfo GetIdProperty()
        {
            return typeof(TEntity)
                       .GetProperties()
                       .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(BsonIdAttribute))) ??
                   throw new ArgumentException(
                       $"There is no property that has [BsonId] attribute in {typeof(TEntity).Name}");
        }

        private static ObjectId ConvertToId(object? id, CancellationToken cancellationToken = default)
        {
            ObjectId? objectId = null;
            if (id != null)
                objectId = id switch
                {
                    string strId => ObjectId.Parse(strId),
                    ObjectId objId => objId,
                    _ => null
                };
            return objectId ??
                   throw new ArgumentException("Id is not a string or ObjectId instance");
        }

        private static FilterDefinition<TEntity> GetIdFilter(ObjectId id, CancellationToken cancellationToken = default)
        {
            return Builders<TEntity>.Filter.Eq("_id", id);
        }


        private async Task<TEntity> FindByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
        {
            FilterDefinition<TEntity> filter = GetIdFilter(id, cancellationToken);
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
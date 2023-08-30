using Application.Common.Exceptions;
using Application.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;

namespace DataAccess
{
    public class SimpleMongoDbSet<TEntity> : ISimpleDbSet<TEntity> where TEntity : class
    {
        public CancellationToken CancellationToken { get; set; } = default;

        private IMongoCollection<TEntity> _collection;

        public SimpleMongoDbSet(IMongoCollection<TEntity> collection, CancellationToken cancellationToken = default)
        {
            _collection = collection;
        }

        public Task<TEntity> FindAsync(object id)
        {
            ObjectId objectId = ConvertToId(id);
            return FindByIdAsync(objectId);
        }

        public Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _collection.Find(expression).ToListAsync(CancellationToken);
        }


        public void AddMany(IEnumerable<TEntity> entities)
        {
            _collection.InsertMany(entities, null, CancellationToken);
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            PropertyInfo idProp = GetIdProperty();

            ObjectId objectId = ObjectId.GenerateNewId();
            idProp.SetValue(entity, objectId.ToString());

            await _collection.InsertOneAsync(entity, null, CancellationToken);
            return await FindByIdAsync(objectId);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            ObjectId id = GetId(entity);
            await _collection.ReplaceOneAsync(
                GetIdFilter(id),
                entity,
                cancellationToken: CancellationToken);
            return await FindByIdAsync(id);
        }

        public async Task DeleteAsync(object id)
        {
            ObjectId objId = ConvertToId(id);
            await _collection.DeleteOneAsync(GetIdFilter(objId), null, CancellationToken);
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            await _collection.DeleteManyAsync(expression, null, CancellationToken);
        }

        public Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _collection.CountDocumentsAsync(expression, null, CancellationToken);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            ObjectId id = GetId(entity);
            await _collection.DeleteOneAsync(GetIdFilter(id), null, CancellationToken);
        }

        private static ObjectId GetId(TEntity entity)
        {
            return ConvertToId(GetIdProperty().GetValue(entity));
        }

        private static PropertyInfo GetIdProperty()
        {
            return typeof(TEntity)
                       .GetProperties()
                       .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(BsonIdAttribute))) ??
                   throw new ArgumentException(
                       $"There is no property that has [BsonId] attribute in {typeof(TEntity).Name}");
        }

        private static ObjectId ConvertToId(object? id)
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

        private static FilterDefinition<TEntity> GetIdFilter(ObjectId id)
        {
            return Builders<TEntity>.Filter.Eq("_id", id);
        }


        private async Task<TEntity> FindByIdAsync(ObjectId id)
        {
            FilterDefinition<TEntity> filter = GetIdFilter(id);
            TEntity result =
                await (await _collection.FindAsync(filter, null, CancellationToken)).FirstOrDefaultAsync(
                    CancellationToken);
            if (result == null)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found");
            return result;
        }
    }
}
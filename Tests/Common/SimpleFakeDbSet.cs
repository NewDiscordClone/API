using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.Common
{
    public class SimpleFakeDbSet<TEntity> : ISimpleDbSet<TEntity> where TEntity : class
    {
        public List<TEntity> Entitites { get; init; }
        public CancellationToken CancellationToken { private get; set; }

        public SimpleFakeDbSet(List<TEntity> init)
        {
            Entitites = init;
        }

        public async Task<TEntity> FindAsync(object id)
        {
            ObjectId objectId = ConvertToId(id);
            return Entitites[FindIndex(objectId)];
        }
        public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Entitites.FindAll(new Predicate<TEntity>(expression.Compile()) ??
                throw new ArgumentException("Expression<Func<TEntity, bool>> is can't be cast to Predicate<TEntity>"));
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            Entitites.AddRange(entities);
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            PropertyInfo idProp = GetIdProperty();

            ObjectId objectId = ObjectId.GenerateNewId();
            idProp.SetValue(entity, objectId.ToString());

            Entitites.Add(entity);
            return Task.FromResult(Entitites[FindIndex(objectId)]);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Entitites[FindIndex(GetId(entity))] = entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Entitites.RemoveAt(FindIndex(GetId(entity)));
        }

        public async Task DeleteAsync(object id)
        {
            Entitites.RemoveAt(FindIndex(ConvertToId(id)));
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            List<TEntity> list = await FilterAsync(expression);
            foreach (TEntity entity in list)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Entitites.Count;
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


        private int FindIndex(ObjectId id)
        {
            int result = Entitites.FindIndex(e => GetId(e) == id);
            if (result < 0)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found", id.ToString());
            return result;
        }
    }
    
    public class SimpleFakeDbSet<TEntity, TKey> : ISimpleDbSet<TEntity> where TEntity : class where TKey : notnull
    {
        public List<TEntity> Entitites { get; init; }
        public CancellationToken CancellationToken { private get; set; }

        public SimpleFakeDbSet(List<TEntity> init)
        {
            Entitites = init;
        }

        public async Task<TEntity> FindAsync(object id)
        {
            TKey TKey = ConvertToId(id);
            return Entitites[FindIndex(TKey)];
        }
        public async Task<List<TEntity>> FilterAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Entitites.FindAll(new Predicate<TEntity>(expression.Compile()) ??
                throw new ArgumentException("Expression<Func<TEntity, bool>> is can't be cast to Predicate<TEntity>"));
        }

        public void AddMany(IEnumerable<TEntity> entities)
        {
            Entitites.AddRange(entities);
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            TKey TKey = GetId(entity);

            Entitites.Add(entity);
            return Task.FromResult(Entitites[FindIndex(TKey)]);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Entitites[FindIndex(GetId(entity))] = entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            Entitites.RemoveAt(FindIndex(GetId(entity)));
        }

        public async Task DeleteAsync(object id)
        {
            Entitites.RemoveAt(FindIndex(ConvertToId(id)));
        }

        public async Task DeleteManyAsync(Expression<Func<TEntity, bool>> expression)
        {
            List<TEntity> list = await FilterAsync(expression);
            foreach (TEntity entity in list)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> expression)
        {
            return Entitites.Count;
        }

        private static TKey GetId(TEntity entity)
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

        private static TKey ConvertToId(object? id)
        {
            if (id is not TKey key) throw new ArgumentException($"Id is not a string or {typeof(TKey).Name} instance");
            return key;
        }

        private int FindIndex(TKey id)
        {
            int result = Entitites.FindIndex(e => GetId(e).Equals(id));
            if (result < 0)
                throw new EntityNotFoundException($"{typeof(TEntity).Name} {id} not found", id.ToString());
            return result;
        }
    }
}
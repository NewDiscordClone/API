using Application.Common.Exceptions;
using Application.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Linq.Expressions;
using System.Reflection;

namespace Tests.Common
{
    public class SimpleTwoFakeDbSets<T, T1, T2> : ISimpleDbSet<T>
        where T : class
        where T1 : class, T
        where T2 : class, T
    {
        public SimpleFakeDbSet<T1> DbSet1 { get; init; }
        public SimpleFakeDbSet<T2> DbSet2 { get; init; }


        public CancellationToken CancellationToken { get; set; }

        public SimpleTwoFakeDbSets(List<T1> init1, List<T2> init2)
        {
            DbSet1 = new SimpleFakeDbSet<T1>(init1);
            DbSet2 = new SimpleFakeDbSet<T2>(init2);
        }

        public async Task<T> FindAsync(object id)
        {
            ObjectId objectId = ConvertToId(id);
            return Combine()[FindIndex(objectId)];
        }

        public async Task<List<T>> FilterAsync(Expression<Func<T, bool>> expression)
        {
            return Combine().FindAll(new Predicate<T>(expression.Compile()) ??
                                     throw new ArgumentException(
                                         "Expression<Func<TEntity, bool>> is can't be cast to Predicate<TEntity>"));
        }

        public async void AddMany(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                if (entity is T1 t1)
                    await DbSet1.AddAsync(t1).WaitAsync(CancellationToken);
                if (entity is T2 t2)
                    await DbSet2.AddAsync(t2).WaitAsync(CancellationToken);
            }
        }

        public async Task<T> AddAsync(T entity)
        {
            ObjectId id = GetId(entity);
            if (entity is T1 t1)
                await DbSet1.AddAsync(t1).WaitAsync(CancellationToken);
            if (entity is T2 t2)
                await DbSet2.AddAsync(t2).WaitAsync(CancellationToken);
            return Combine()[FindIndex(id)];
        }

        public async Task<T> UpdateAsync(T entity)
        {
            ObjectId objectId = GetId(entity);
            if (entity is T1 t1)
                await DbSet1.UpdateAsync(t1).WaitAsync(CancellationToken);
            if (entity is T2 t2)
                await DbSet2.UpdateAsync(t2).WaitAsync(CancellationToken);
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            ObjectId objectId = GetId(entity);
            if (entity is T1 t1)
                await DbSet1.DeleteAsync(t1).WaitAsync(CancellationToken);
            if (entity is T2 t2)
                await DbSet2.DeleteAsync(t2).WaitAsync(CancellationToken);
        }

        public async Task DeleteAsync(object id)
        {
            ObjectId objectId = ConvertToId(id);
            T entity = Combine()[FindIndex(objectId)];
            if (entity is T1 t1)
                await DbSet1.DeleteAsync(t1).WaitAsync(CancellationToken);
            if (entity is T2 t2)
                await DbSet2.DeleteAsync(t2).WaitAsync(CancellationToken);
        }

        public async Task DeleteManyAsync(Expression<Func<T, bool>> expression)
        {
            List<T> list = await FilterAsync(expression);
            foreach (T entity in list)
            {
                await DeleteAsync(entity);
            }
        }

        public async Task<long> CountAsync(Expression<Func<T, bool>> expression)
        {
            return Combine().Count;
        }

        private static ObjectId GetId(T entity)
        {
            return ConvertToId(GetIdProperty().GetValue(entity));
        }

        private static PropertyInfo GetIdProperty()
        {
            return typeof(T)
                       .GetProperties()
                       .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(BsonIdAttribute))) ??
                   throw new ArgumentException(
                       $"There is no property that has [BsonId] attribute in {typeof(T).Name}");
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

        private List<T> Combine()
        {
            return DbSet1.Entitites.Cast<T>().Concat(DbSet2.Entitites).ToList();
        }

        private int FindIndex(ObjectId id, int list = 0)
        {
            int result = (list switch
            {
                0 => Combine(),
                1 => DbSet1.Entitites.Cast<T>().ToList(),
                2 => DbSet2.Entitites.Cast<T>().ToList(),
                _ => throw new Exception()
            }).FindIndex(e => GetId(e) == id);
            if (result < 0)
                throw new EntityNotFoundException($"{typeof(T).Name} {id} not found", id.ToString());
            return result;
        }
    }
}
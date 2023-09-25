//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using Sparkle.Application.Common.Exceptions;
//using Sparkle.Application.Common.Interfaces;
//using System.Linq.Expressions;
//using System.Reflection;

//namespace Sparkle.Tests.Common
//{
//    public class SimpleDerivedFakeDbSet<T, TD> : ISimpleDbSet<T, TD>
//        where TD : class, T
//        where T : class
//    {
//        public CancellationToken CancellationToken { get; set; } = CancellationToken.None;

//        private SimpleFakeDbSet<T> _baseSet;
//        public List<TD> Entitites => _baseSet.Entitites.OfType<TD>().ToList();

//        public SimpleDerivedFakeDbSet(SimpleFakeDbSet<T> baseSet)
//        {
//            _baseSet = baseSet;
//        }

//        public async Task<TD> FindAsync(object id)
//        {
//            ObjectId objectId = ConvertToId(id);
//            return Entitites[FindIndex(objectId)];
//        }

//        public async Task<List<TD>> FilterAsync(Expression<Func<TD, bool>> expression)
//        {
//            return Entitites.FindAll(new Predicate<TD>(expression.Compile()) ??
//                                     throw new ArgumentException(
//                                         "Expression<Func<TEntity, bool>> is can't be cast to Predicate<TEntity>"));
//        }

//        public void AddMany(IEnumerable<TD> entities)
//        {
//            _baseSet.AddMany(entities);
//        }

//        public async Task<TD> AddAsync(TD entity)
//        {
//            return await _baseSet.AddAsync(entity) as TD ?? throw new Exception();
//        }

//        public async Task<TD> UpdateAsync(TD entity)
//        {
//            return await _baseSet.UpdateAsync(entity) as TD ?? throw new Exception();
//        }

//        public Task DeleteAsync(TD entity)
//        {
//            return _baseSet.DeleteAsync(entity);
//        }

//        public Task DeleteAsync(object id)
//        {
//            return _baseSet.DeleteAsync(id);
//        }

//        public async Task DeleteManyAsync(Expression<Func<TD, bool>> expression)
//        {
//            List<TD> list = await FilterAsync(expression);
//            foreach (TD entity in list)
//            {
//                await DeleteAsync(entity);
//            }
//        }

//        public async Task<long> CountAsync(Expression<Func<TD, bool>> expression)
//        {
//            return Entitites.Count;
//        }

//        private static ObjectId GetId(TD entity)
//        {
//            return ConvertToId(GetIdProperty().GetValue(entity));
//        }

//        private static PropertyInfo GetIdProperty()
//        {
//            return typeof(TD)
//                       .GetProperties()
//                       .FirstOrDefault(prop => Attribute.IsDefined(prop, typeof(BsonIdAttribute))) ??
//                   throw new ArgumentException(
//                       $"There is no property that has [BsonId] attribute in {typeof(TD).Name}");
//        }

//        private static ObjectId ConvertToId(object? id)
//        {
//            ObjectId? objectId = null;
//            if (id != null)
//                objectId = id switch
//                {
//                    string strId => ObjectId.Parse(strId),
//                    ObjectId objId => objId,
//                    _ => null
//                };
//            return objectId ??
//                   throw new ArgumentException("Id is not a string or ObjectId instance");
//        }


//        private int FindIndex(ObjectId id)
//        {
//            int result = Entitites.FindIndex(e => GetId(e) == id);
//            if (result < 0)
//                throw new EntityNotFoundException($"{typeof(TD).Name} {id} not found", id.ToString());
//            return result;
//        }
//    }
//}
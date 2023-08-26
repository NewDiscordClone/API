using MongoDB.Bson;

namespace Tests.Common
{
    public class Ids
    {
        public ObjectId UserAId { get; set; }
        public ObjectId UserBId { get; set; }
        public ObjectId UserCId { get; set; }
        public ObjectId UserDId { get; set; }
        public ObjectId UserFailId { get; set; }

        public ObjectId ServerIdForDelete { get; set; }
        public ObjectId ServerIdForUpdate { get; set; }

        public ObjectId Channel1 { get; set; }
        public ObjectId Channel2 { get; set; }

        public ObjectId PrivateChat3 { get; set; }
        public ObjectId PrivateChat4 { get; set; }
        public ObjectId PrivateChat5 { get; set; }
        public ObjectId PrivateChat6 { get; set; }
        public ObjectId PrivateChat7 { get; set; } 
        
        public ObjectId Message1 { get; set; } 
        public ObjectId Message2 { get; set; } 
    }
}
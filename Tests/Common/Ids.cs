using MongoDB.Bson;

namespace Tests.Common
{
    public class Ids
    {
        public int UserAId { get; set; } = 1;
        public int UserBId { get; set; } = 2;
        public int UserCId { get; set; } = 3;
        public int UserDId { get; set; } = 4;

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
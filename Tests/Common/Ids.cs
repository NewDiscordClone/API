using MongoDB.Bson;

namespace Tests.Common
{
    public class Ids
    {
        public Guid UserAId { get; set; }
        public Guid UserBId { get; set; }
        public Guid UserCId { get; set; }
        public Guid UserDId { get; set; }

        public string ServerIdForDelete { get; set; }
        public string ServerIdForUpdate { get; set; }

        public string Channel1 { get; set; }
        public string Channel2 { get; set; }

        public string GroupChat3 { get; set; }
        public string GroupChat4 { get; set; }
        public string GroupChat5 { get; set; }
        public string GroupChat6 { get; set; }
        public string GroupChat7 { get; set; } 
        
        public string Message1 { get; set; } 
        public string Message2 { get; set; } 
    }
}
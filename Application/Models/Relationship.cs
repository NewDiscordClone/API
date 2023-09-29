namespace Sparkle.Application.Models
{
    public enum RelationshipTypes
    {
        Acquaintance,
        Friend,
        Pending,
        Waiting,
        Blocked
    }
    public class Relationship
    {
        public Guid UserActive { get; set; }
        public Guid UserPassive { get; set; }
        public RelationshipTypes RelationshipType { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Relationship relationship)
            {
                return relationship.UserActive == UserActive && relationship.UserPassive == UserPassive;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(UserActive, UserPassive);
        }
    }
}
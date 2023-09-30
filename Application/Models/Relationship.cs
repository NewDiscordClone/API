namespace Sparkle.Application.Models
{
    public enum RelationshipTypes
    {
        Acquaintance,
        Friend,
        Pending,
        Blocked
    }
    public class Relationship
    {
        public Guid Active { get; set; }
        public Guid Passive { get; set; }
        public RelationshipTypes RelationshipType { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Relationship relationship)
            {
                return relationship.Active == Active && relationship.Passive == Passive;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Active, Passive);
        }
    }
}
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
        public string? PersonalChatId { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Relationship relationship)
            {
                return relationship.Active == Active && relationship.Passive == Passive;
            }
            return false;
        }

        public static bool operator ==(Relationship? left, Relationship? right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        public static bool operator !=(Relationship? left, Relationship? right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Active, Passive);
        }

        public override string ToString()
        {
            return $"Active: {Active}, Passive: {Passive}, RelationshipType: {RelationshipType}";
        }

        public void SwapUsers() => (Passive, Active) = (Active, Passive);
    }
}
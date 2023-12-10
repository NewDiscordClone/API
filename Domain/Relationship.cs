namespace Sparkle.Domain
{
    public enum RelationshipTypes
    {
        Acquaintance,
        Friend,
        Pending,
        Blocked
    }
    /// <summary>
    /// Represents a relationship between two users.
    /// </summary>
    public class Relationship
    {
        /// <summary>
        /// Gets or sets the ID of the active user in the relationship.
        /// </summary>
        public Guid Active { get; set; }

        /// <summary>
        /// Gets or sets the ID of the passive user in the relationship.
        /// </summary>
        public Guid Passive { get; set; }

        /// <summary>
        /// Gets or sets the type of relationship.
        /// </summary>
        public RelationshipTypes RelationshipType { get; set; }

        /// <summary>
        /// Gets or sets the ID of the personal chat associated with the relationship.
        /// </summary>
        public string? PersonalChatId { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object? obj)
        {
            if (obj is Relationship relationship)
            {
                return relationship.Active == Active && relationship.Passive == Passive;
            }
            return false;
        }

        /// <summary>
        /// Determines whether two specified Relationship objects have the same value.
        /// </summary>
        /// <param name="left">The first Relationship to compare, or null.</param>
        /// <param name="right">The second Relationship to compare, or null.</param>
        /// <returns>true if the value of left is the same as the value of right; otherwise, false.</returns>
        public static bool operator ==(Relationship? left, Relationship? right)
        {
            if (left is null)
            {
                return right is null;
            }
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether two specified Relationship objects have different values.
        /// </summary>
        /// <param name="left">The first Relationship to compare, or null.</param>
        /// <param name="right">The second Relationship to compare, or null.</param>
        /// <returns>true if the value of left is different from the value of right; otherwise, false.</returns>
        public static bool operator !=(Relationship? left, Relationship? right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Determines whether the specified Relationship object has the same RelationshipType value as the specified RelationshipTypes value.
        /// </summary>
        /// <param name="left">The Relationship to compare, or null.</param>
        /// <param name="right">The RelationshipTypes value to compare, or null.</param>
        /// <returns>true if the RelationshipType value of left is the same as the value of right; otherwise, false.</returns>
        public static bool operator ==(Relationship? left, RelationshipTypes right)
        {
            return left?.RelationshipType == right;
        }

        /// <summary>
        /// Determines whether the specified Relationship object has a different RelationshipType value than the specified RelationshipTypes value.
        /// </summary>
        /// <param name="left">The Relationship to compare, or null.</param>
        /// <param name="right">The RelationshipTypes value to compare, or null.</param>
        /// <returns>true if the RelationshipType value of left is different from the value of right; otherwise, false.</returns>
        public static bool operator !=(Relationship? left, RelationshipTypes right)
        {
            return left?.RelationshipType != right;
        }

        /// <summary>
        /// Returns a hash code for the current Relationship object.
        /// </summary>
        /// <returns>A hash code for the current Relationship object.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Active, Passive);
        }

        /// <summary>
        /// Returns a string that represents the current Relationship object.
        /// </summary>
        /// <returns>A string that represents the current Relationship object.</returns>
        public override string ToString()
        {
            return $"Active: {Active}, Passive: {Passive}, RelationshipType: {RelationshipType}";
        }

        /// <summary>
        /// Swaps the <see cref="Active"/> and <see cref="Passive"/> properties of the Relationship object.
        /// </summary>
        public void SwapUsers() => (Passive, Active) = (Active, Passive);
    }
}
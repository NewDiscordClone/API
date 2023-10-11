using Sparkle.Application.Models;

namespace Sparkle.Application.Users.Relationships.Queries.GetRelationships
{
    /// <summary>
    /// Represents a relationship view model.
    /// </summary>
    public record RelationshipViewModel
    {

        /// <summary>
        /// Gets or sets a value indicating whether the relationship is active.
        /// </summary>
        /// <remarks>If <see langword="true"/>: current user is passive user</remarks>
        public bool IsActive { get; init; }

        /// <summary>
        /// Gets or sets the user lookup view model.
        /// </summary>
        public UserLookupViewModel User { get; init; }

        /// <summary>
        /// Gets or sets the relationship type.
        /// </summary>
        public RelationshipTypes Type { get; init; }
        /// <summary>
        /// Id of the chat between two users.
        /// </summary>
        public string? ChatId { get; init; }
    }
}

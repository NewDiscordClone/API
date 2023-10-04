using Sparkle.Application.Models;
using Sparkle.Application.Models.LookUps;
using Sparkle.Application.Users.Relationships.Queries.GetRelationships;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IConvertor
    {
        /// <summary>
        /// Converts the specified relationship to a relationship view model.
        /// </summary>
        /// <param name="relationship">The relationship to convert.</param>
        /// <returns>The relationship view model.</returns>
        RelationshipViewModel Convert(Relationship relationship);

        Task<PrivateChatLookUp> ConvertAsync(PersonalChat chat, CancellationToken cancellationToken = default);
        PrivateChatLookUp Convert(PersonalChat chat);
        // PrivateChatViewModel Convert(PersonalChat chat);
    }
}

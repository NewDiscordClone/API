using Sparkle.Domain;
using Sparkle.Domain.LookUps;
using Sparkle.Application.Users.Relationships.Queries;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IConvertor
    {
        /// <summary>
        /// Converts the specified relationship to a relationship view model.
        /// </summary>
        /// <param name="relationship">The relationship to convert.</param>
        /// <param name="userId">Id of the user who get view model. Current user's id by defauls</param>
        /// <returns>The relationship view model.</returns>
        RelationshipViewModel Convert(Relationship relationship, Guid? userId = null);

        Task<PrivateChatLookUp> ConvertAsync(PersonalChat chat, CancellationToken cancellationToken = default);
        Task<PrivateChatViewModel> ConvertToViewModelAsync(PersonalChat chat, CancellationToken cancellationToken = default);
        PrivateChatLookUp Convert(PersonalChat chat);

        Task<string> FillChatTitleAsync(List<Guid> userIds, CancellationToken cancellationToken = default);
        Task<string> FillChatTitleAsync(List<Guid> userIds, Guid receiverId, CancellationToken cancellationToken);
    }
}

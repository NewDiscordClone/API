using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IAuthorizedUserProvider
    {
        Guid GetUserId();
        bool IsAdmin(UserProfile profile);
        bool HasClaims(UserProfile profile, IEnumerable<string> claimTypes);
        bool HasClaims(UserProfile profile, params string[] claimTypes);
        Task<bool> HasClaimsAsync(UserProfile profile, IEnumerable<string> claimTypes);
        Task<bool> HasClaimsAsync(UserProfile profile, params string[] claimTypes);
    }
}
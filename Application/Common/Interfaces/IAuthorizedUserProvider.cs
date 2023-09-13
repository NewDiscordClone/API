namespace Sparkle.Application.Common.Interfaces
{
    public interface IAuthorizedUserProvider
    {
        Guid GetUserId();
        bool IsAdmin(string serverId);
        Task<bool> IsAdminAsync(string serverId);
        bool HasClaims(string serverId, IEnumerable<string> claimTypes);
        bool HasClaims(string serverId, params string[] claimTypes);
        Task<bool> HasClaimsAsync(string serverId, IEnumerable<string> claimTypes);
        Task<bool> HasClaimsAsync(string serverId, params string[] claimTypes);
    }
}
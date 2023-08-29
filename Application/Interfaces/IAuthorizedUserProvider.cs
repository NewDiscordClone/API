namespace Application.Providers
{
    public interface IAuthorizedUserProvider
    {
        int GetUserId();
        bool HasClaims(int serverId, IEnumerable<string> claimTypes);
        bool HasClaims(int serverId, params string[] claimTypes);
        Task<bool> HasClaimsAsync(int serverId, IEnumerable<string> claimTypes);
        Task<bool> HasClaimsAsync(int serverId, params string[] claimTypes);
    }
}
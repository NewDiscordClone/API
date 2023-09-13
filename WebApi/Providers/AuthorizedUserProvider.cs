using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace Sparkle.WebApi.Providers
{
    public class AuthorizedUserProvider : IAuthorizedUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbContext _context;
        private ClaimsPrincipal User;

        public AuthorizedUserProvider(IHttpContextAccessor httpContextAccessor, IAppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            User = httpContextAccessor.HttpContext?.User;
        }

        public Guid GetUserId()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdClaim)) return Guid.Parse(userIdClaim);

            throw new NoSuchUserException();
        }

        public void SetUser(ClaimsPrincipal user)
        {
            User = user;
        }

        public async Task<bool> HasClaimsAsync(string serverId, IEnumerable<string> claimTypes)
        {
            Server? server = await _context.Servers.FindAsync(serverId);

            if (server is null)
                return false;

            ServerProfile? profile = server.ServerProfiles.
                FirstOrDefault(profile => profile.UserId == GetUserId());


            if (profile is null || profile.Roles is null)
                return false;

            List<string> matchingUserClaims = new();
            foreach (Role role in profile.Roles.OrderByDescending(role => role.Priority))
            {
                foreach (Claim claim in await _context.GetRoleClaimAsync(role))
                {
                    if (claimTypes.Any(claimType => string.Equals(claim.Type, claimType)))
                    {
                        if (!bool.Parse(claim.Value) && !matchingUserClaims.Contains(claim.Type))
                            return false;

                        matchingUserClaims.Add(claim.Type);
                    }

                    if (claimTypes.All(matchingUserClaims.Contains))
                    {
                        return true;
                    }
                }
            }
            return true;
        }

        public async Task<bool> HasClaimsAsync(string serverId, params string[] claimTypes)
        {
            if (claimTypes is null)
            {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            return await HasClaimsAsync(serverId, (IEnumerable<string>)claimTypes);
        }

        public bool HasClaims(string serverId, params string[] claimTypes)
        {
            if (claimTypes is null)
            {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            return HasClaims(serverId, (IEnumerable<string>)claimTypes);
        }

        public bool HasClaims(string serverId, IEnumerable<string> claimTypes)
        {
            return HasClaimsAsync(serverId, claimTypes).Result;
        }

        public bool IsAdmin(string serverId)
        {
            return IsAdminAsync(serverId).Result;
        }

        public async Task<bool> IsAdminAsync(string serverId)
        {
            Server? server = await _context.Servers.FindAsync(serverId);

            if (server is null)
                return false;

            ServerProfile? profile = server.ServerProfiles.
                FirstOrDefault(profile => profile.UserId == GetUserId());


            if (profile is null || profile.Roles is null)
                return false;

            bool isAdmin = profile.Roles.Any(role => role.IsAdmin);
            return isAdmin;
        }
    }
}
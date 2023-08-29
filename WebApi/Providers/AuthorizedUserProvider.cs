using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace WebApi.Providers
{
    public class AuthorizedUserProvider : IAuthorizedUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbContext _context;
        private readonly RoleManager<Role> _roleManager;

        public AuthorizedUserProvider(IHttpContextAccessor httpContextAccessor, IAppDbContext context, RoleManager<Role> roleManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _roleManager = roleManager;
        }

        public int GetUserId()
        {
            string? userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new NoSuchUserException();
            }

            return int.Parse(userIdClaim);
        }
        public async Task<bool> HasClaimsAsync(string serverId, IEnumerable<string> claimTypes)
        {
            Server? server = await _context.Servers.FindAsync(serverId);

            if (server is null)
                return false;

            ServerProfile? profile = server.ServerProfiles.
                FirstOrDefault(profile => profile.User.Id == GetUserId());


            if (profile is null || profile.Roles is null)
                return false;

            List<string> matchingUserClaims = new();
            foreach (Role role in profile.Roles)
            {
                foreach (Claim claim in await _roleManager.GetClaimsAsync(role))
                {
                    if (claimTypes.Any(claimType
                        => string.Equals(claim.Type, claimType)))
                    {
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
                FirstOrDefault(profile => profile.User.Id == GetUserId());


            if (profile is null || profile.Roles is null)
                return false;

            bool isAdmin = profile.Roles.Any(role => role.IsAdmin);
            return isAdmin;
        }
    }
}
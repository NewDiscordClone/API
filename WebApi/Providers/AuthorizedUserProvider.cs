using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Domain;
using System.Security.Claims;

namespace Sparkle.WebApi.Providers
{
    public class AuthorizedUserProvider : IAuthorizedUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoleRepository _roleRepository;
        private ClaimsPrincipal User;

        public AuthorizedUserProvider(IHttpContextAccessor httpContextAccessor, IRoleRepository roleRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            User = httpContextAccessor.HttpContext?.User;
            //?? throw new Exception("User not authenticated");
            _roleRepository = roleRepository;
        }

        public Guid GetUserId()
        {
            string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrEmpty(userIdClaim))
                return Guid.Parse(userIdClaim);

            throw new NoSuchUserException();
        }

        public string GetUserName()
        {
            IEnumerable<Claim> claims = User.Claims;
            string? userNameClaim = claims.FirstOrDefault(c => c.Type == "name")?.Value;
            if (!string.IsNullOrEmpty(userNameClaim))
                return userNameClaim;

            throw new NoSuchUserException();
        }

        public void SetUser(ClaimsPrincipal user)
        {
            User = user;
        }

        public async Task<bool> HasClaimsAsync(UserProfile profile, IEnumerable<string> claimTypes)
        {
            if (profile is null || profile.Roles is null)
                return false;

            List<string> matchingUserClaims = new();
            IOrderedEnumerable<Role> roles = profile.Roles.OrderByDescending(role => role.Priority);

            foreach (Role role in roles)
            {
                List<IdentityRoleClaim<Guid>> identityClaims = await _roleRepository.GetRoleClaimAsync(role);
                List<Claim> claims = identityClaims.ConvertAll(claim => claim.ToClaim());

                foreach (Claim claim in claims)
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
            return false;
        }

        public async Task<bool> HasClaimsAsync(UserProfile profile, params string[] claimTypes)
        {
            if (claimTypes is null)
            {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            return await HasClaimsAsync(profile, (IEnumerable<string>)claimTypes);
        }

        public bool HasClaims(UserProfile profile, params string[] claimTypes)
        {
            if (claimTypes is null)
            {
                throw new ArgumentNullException(nameof(claimTypes));
            }

            return HasClaims(profile, (IEnumerable<string>)claimTypes);
        }

        public bool HasClaims(UserProfile profile, IEnumerable<string> claimTypes)
        {
            return HasClaimsAsync(profile, claimTypes).Result;
        }

        public bool IsAdmin(UserProfile profile)
        {
            if (profile is null || profile.Roles is null)
                return false;

            bool isAdmin = profile.Roles.Any(role => role.IsAdmin);
            return isAdmin;
        }
    }
}
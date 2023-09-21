﻿using Microsoft.AspNetCore.Identity;
using Sparkle.Application.Common.Exceptions;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using System.Security.Claims;

namespace Sparkle.WebApi.Providers
{
    public class AuthorizedUserProvider : IAuthorizedUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbContext _context;

        public AuthorizedUserProvider(IHttpContextAccessor httpContextAccessor, IAppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public Guid GetUserId()
        {
            string? userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new NoSuchUserException();
            }

            return Guid.Parse(userIdClaim);
        }
        public async Task<bool> HasClaimsAsync(UserProfile profile, IEnumerable<string> claimTypes)
        {
            if (profile is null || profile.Roles is null)
                return false;

            List<string> matchingUserClaims = new();
            foreach (Role role in profile.Roles.OrderByDescending(role => role.Priority))
            {
                foreach (IdentityRoleClaim<Guid> identityClaim in await _context.GetRoleClaimAsync(role))
                {
                    Claim claim = identityClaim.ToClaim();
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
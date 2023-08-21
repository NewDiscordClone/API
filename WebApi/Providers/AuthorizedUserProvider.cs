using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebApi.Providers
{
    public class AuthorizedUserProvider : IAuthorizedUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppDbContext _context;

        public AuthorizedUserProvider(IHttpContextAccessor httpContextAccessor, IAppDbContext appDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = appDbContext;
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

        public bool IsInRole(Role role, Server server)
        {
            if (server.Roles.Contains(role))
                return false;

            ServerProfile? profile = server.ServerProfiles
                .FirstOrDefault(user => user.Id == GetUserId());

            if (profile is null)
                return false;

            return profile.Roles.Contains(role);
        }

        public bool IsInRole(string roleName, int serverId)
        {
            return IsInRoleAsync(roleName, serverId).Result;
        }

        public async Task<bool> IsInRoleAsync(string roleName, int serverId)
        {
            Role? role = await _context.Roles.FirstOrDefaultAsync(role
                => role.Server.Id == serverId && role.Name == roleName);

            Server? server = await _context.Servers.FindAsync(serverId);

            if (role is null || server is null)
                throw new EntityNotFoundException("Role or Server not found");

            return IsInRole(role, server);
        }
    }
}
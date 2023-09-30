using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    internal class RoleRepository : BaseSqlRepository<Role, Guid>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }
        public async Task AddClaimsToRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims, CancellationToken cancellationToken = default)
        {
            foreach (IdentityRoleClaim<Guid> claim in claims)
            {
                claim.RoleId = role.Id;
                await Context.RoleClaims.AddAsync(claim, cancellationToken);
            }
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task AddClaimToRoleAsync(Role role, IdentityRoleClaim<Guid> claim, CancellationToken cancellationToken = default)
        {
            await AddClaimsToRoleAsync(role, new List<IdentityRoleClaim<Guid>> { claim }, cancellationToken);
        }

        public async Task<List<IdentityRoleClaim<Guid>>> GetRoleClaimAsync(Role role, CancellationToken cancellationToken = default)
        {
            return await Context.RoleClaims
                .Where(t => t.RoleId == role.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<Role> GetServerMemberRoleAsync(string serverId, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .SingleAsync(role => role.ServerId == serverId
                && role.Name == Constants.ServerProfile.DefaultMemberRoleName,
                cancellationToken);
        }

        public bool IsPriorityUniqueInServer(string serverId, int priority)
        {
            return !DbSet.Any(role => role.ServerId == serverId && priority == role.Priority);
        }

        public async Task RemoveClaimFromRoleAsync(Role role, IdentityRoleClaim<Guid> claim, CancellationToken cancellationToken = default)
        {
            Context.RoleClaims.Remove(claim);

            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveClaimsFromRoleAsync(Role role, IEnumerable<IdentityRoleClaim<Guid>> claims, CancellationToken cancellationToken = default)
        {
            for (int i = claims.Count() - 1; i >= 0; i--)
            {
                IdentityRoleClaim<Guid> claim = claims.ElementAt(i);

                await RemoveClaimFromRoleAsync(role, claim, cancellationToken);
            }
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task RemoveClaimsFromRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            List<IdentityRoleClaim<Guid>> claimToRemove = await Context.RoleClaims
               .Where(rc => rc.RoleId == role.Id).ToListAsync(cancellationToken);

            Context.RoleClaims.RemoveRange(claimToRemove);
            await Context.SaveChangesAsync(cancellationToken);
        }

    }
}

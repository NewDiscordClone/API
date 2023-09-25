using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    internal class RoleRepository : SimpleSqlDbSet<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context, default)
        {
        }

        public async Task<Role> GetServerMemberRoleAsync(string serverId)
        {
            return await DbSet
                .SingleAsync(role => role.ServerId == serverId
                && role.Name == Constants.ServerProfile.DefaultMemberRoleName);
        }
    }
}

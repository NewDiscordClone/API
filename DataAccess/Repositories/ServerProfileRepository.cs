using Microsoft.EntityFrameworkCore;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.DataAccess.Repositories
{
    public class ServerProfileRepository : SimpleSqlDbSet<ServerProfile>, IServerProfileRepository
    {
        public ServerProfileRepository(AppDbContext context) : base(context, default)
        {
        }

        public async Task RemoveRoleFromServerProfilesAsync(Role role, string serverId, CancellationToken cancellationToken = default)
        {
            await DbSet.Where(profile => profile.ServerId == serverId && profile.Roles.Contains(role))
                 .ForEachAsync(profile => profile.Roles.Remove(role), cancellationToken);
        }
    }
}

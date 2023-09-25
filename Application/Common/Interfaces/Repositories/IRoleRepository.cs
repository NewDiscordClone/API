using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces.Repositories
{
    public interface IRoleRepository : ISimpleDbSet<Role, Guid>
    {
        Task<Role> GetServerMemberRoleAsync(string serverId);
    }
}

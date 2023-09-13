using Sparkle.Application.Models;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IRoleFactory
    {
        List<Role> GetDefaultServerRoles(string serverId);
    }
}

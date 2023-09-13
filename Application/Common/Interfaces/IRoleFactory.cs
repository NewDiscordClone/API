using Application.Models;

namespace Application.Common.Interfaces
{
    public interface IRoleFactory
    {
        List<Role> GetDefaultServerRoles(string serverId);
    }
}

using Application.Models;

namespace Application.Interfaces
{
    public interface IRoleFactory
    {
        List<Role> GetDefaultServerRoles(string serverId);
    }
}

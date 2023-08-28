using System.Security.Claims;

namespace Application.Interfaces
{
    public interface IAuthorizedUserProvider
    {
        int GetUserId();
    }
}
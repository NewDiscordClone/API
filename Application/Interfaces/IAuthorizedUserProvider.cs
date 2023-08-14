using System.Security.Claims;

namespace Application.Providers
{
    public interface IAuthorizedUserProvider
    {
        int GetUserId();
    }
}
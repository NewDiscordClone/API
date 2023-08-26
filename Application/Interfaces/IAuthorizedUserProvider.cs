using System.Security.Claims;
using MongoDB.Bson;

namespace Application.Providers
{
    public interface IAuthorizedUserProvider
    {
        ObjectId GetUserId();
    }
}
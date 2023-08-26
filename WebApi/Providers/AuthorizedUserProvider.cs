using System.Security.Claims;
using Application.Exceptions;
using Application.Providers;
using MongoDB.Bson;

namespace WebApi.Providers
{
    public class AuthorizedUserProvider : IAuthorizedUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizedUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ObjectId GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new NoSuchUserException();
            }

            if (!ObjectId.TryParse(userIdClaim, out var resultId))
            {
                throw new Exception("Can't parse userIdClaim");
            }

            return resultId;
        }
    }
}
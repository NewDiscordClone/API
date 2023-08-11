using System.Security.Claims;
using Application.Exceptions;
using Application.Providers;

namespace WebApi.Providers
{
    public class AuthorizedUserProvider : IAuthorizedUserProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizedUserProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new NoSuchUserException();
            }

            return int.Parse(userIdClaim);
        }
    }
}
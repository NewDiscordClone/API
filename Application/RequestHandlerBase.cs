using Application.Interfaces;
using Application.Providers;

namespace Application
{
    public abstract class RequestHandlerBase
    {
        protected readonly IAppDbContext Context;
        private readonly IAuthorizedUserProvider _userProvider;
        protected int UserId => _userProvider.GetUserId();

        public RequestHandlerBase(IAppDbContext context, IAuthorizedUserProvider userProvider)
        {
            Context = context;
            _userProvider = userProvider;
        }
    }
}
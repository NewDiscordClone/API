using AutoMapper;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application
{
    public abstract class RequestHandler
    {
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper? _mapper;
        protected Guid UserId => UserProvider.GetUserId();

        public IAuthorizedUserProvider UserProvider => _userProvider;
        protected IMapper Mapper => _mapper ?? throw new Exception("There is no constructor with IMapper");

        public RequestHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
        public RequestHandler(IAuthorizedUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public RequestHandler(IAuthorizedUserProvider userProvider, IMapper mapper)
        {
            _userProvider = userProvider;
            _mapper = mapper;
        }
    }
}
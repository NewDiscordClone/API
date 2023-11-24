using AutoMapper;
using Sparkle.Application.Common.Interfaces;

namespace Sparkle.Application
{
    public abstract class RequestHandlerBase
    {
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper? _mapper;
        protected Guid UserId => UserProvider.GetUserId();

        public IAuthorizedUserProvider UserProvider => _userProvider;
        protected IMapper Mapper => _mapper ?? throw new Exception("There is no constructor with IMapper");

        public RequestHandlerBase(IMapper mapper)
        {
            _mapper = mapper;
        }
        public RequestHandlerBase(IAuthorizedUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        public RequestHandlerBase(IAuthorizedUserProvider userProvider, IMapper mapper)
        {
            _userProvider = userProvider;
            _mapper = mapper;
        }
    }
}
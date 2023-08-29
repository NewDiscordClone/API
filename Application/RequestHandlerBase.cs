using Application.Interfaces;
using AutoMapper;

namespace Application
{
    public abstract class RequestHandlerBase
    {
        protected readonly IAppDbContext Context;
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper? _mapper;
        protected IMapper Mapper => _mapper ?? throw new Exception("There is no constructor with IMapper");
        protected int UserId => UserProvider.GetUserId();

        public IAuthorizedUserProvider UserProvider => _userProvider;

        public RequestHandlerBase(IAppDbContext context, IAuthorizedUserProvider userProvider)
        {
            Context = context;
            _userProvider = userProvider;
        }

        public RequestHandlerBase(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
        {
            Context = context;
            _userProvider = userProvider;
            _mapper = mapper;
        }
    }
}
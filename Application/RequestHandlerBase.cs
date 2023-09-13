using Application.Interfaces;
using Application.Providers;
using AutoMapper;

namespace Application
{
    public abstract class RequestHandlerBase
    {
        private readonly IAppDbContext _context;
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper? _mapper;

        protected IAppDbContext Context => _context ?? throw new Exception("There is no constructor with IAppDbContext");
        protected Guid UserId => UserProvider.GetUserId();

        public IAuthorizedUserProvider UserProvider => _userProvider;
        protected IMapper Mapper => _mapper ?? throw new Exception("There is no constructor with IMapper");
        public RequestHandlerBase(IAppDbContext context)
        {
            _context = context;
        }
        public RequestHandlerBase(IAuthorizedUserProvider userProvider)
        {
            _userProvider = userProvider;
        }
        public RequestHandlerBase(IAppDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public RequestHandlerBase(IAppDbContext context, IAuthorizedUserProvider userProvider)
        {
            _context = context;
            _userProvider = userProvider;
        }

        public RequestHandlerBase(IAppDbContext context, IAuthorizedUserProvider userProvider, IMapper mapper)
        {
            _context = context;
            _userProvider = userProvider;
            _mapper = mapper;
        }
    }
}
using Application.Interfaces;
using Application.Providers;
using AutoMapper;
using MongoDB.Bson;

namespace Application
{
    public abstract class RequestHandlerBase
    {
        protected readonly IAppDbContext Context;
        private readonly IAuthorizedUserProvider _userProvider;
        private readonly IMapper? _mapper;
        protected IMapper Mapper => _mapper ?? throw new Exception("There is no constructor with IMapper");
        protected ObjectId UserId => _userProvider.GetUserId();

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
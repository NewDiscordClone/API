using Application.Common.Mapping;
using Application.Interfaces;
using Application.Providers;
using AutoMapper;
using MediatR;

namespace Tests.Common
{
    public class TestBase : IDisposable
    {
        private readonly Mock<IAuthorizedUserProvider> _userProvider = new();
        private readonly Mock<IMediator> _mediator = new();

        protected Ids Ids;
        protected IAppDbContext Context;
        protected readonly IMapper Mapper;
        protected IAuthorizedUserProvider UserProvider => _userProvider.Object;
        protected IMediator Mediator => _mediator.Object;
        protected readonly CancellationToken CancellationToken = CancellationToken.None;

        public TestBase()
        {
            MapperConfiguration mapperConfig = new(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly)));
            Mapper = mapperConfig.CreateMapper();
        }

        protected void SetAuthorizedUserId(Guid id)
        {
            _userProvider.Setup(provider => provider.GetUserId()).Returns(id);
        }

        public void CreateDatabase()
        {
            Context = TestDbContextFactory.CreateFake(out Ids);
            //Can be turned to
            //Context = TestDbContextFactory.Create(out Ids); 
            //for real database instances
        }

        public void Dispose()
        {
            TestDbContextFactory.Destroy(Context);
        }

        public void AddMediatorHandler<TRequest>(TRequest request, IRequestHandler<TRequest> handler)
        where TRequest : IRequest
        {
            _mediator.Setup(mediator => mediator.Send(It.IsAny<TRequest>(), CancellationToken))
                .Returns(handler.Handle(request, CancellationToken));
        }
        public void AddMediatorHandler<TRequest, TResult>(TRequest request, IRequestHandler<TRequest, TResult>  handler)
            where TRequest : IRequest<TResult>
        {
            _mediator.Setup(mediator => mediator.Send(It.IsAny<TRequest>(), CancellationToken))
                .Returns(handler.Handle(request, CancellationToken));
        }
    }
}

using Application.Common.Mapping;
using Application.Interfaces;
using Application.Providers;
using AutoMapper;

namespace Tests.Common
{
    public class TestBase : IDisposable
    {
        private readonly Mock<IAuthorizedUserProvider> _userProvider;

        protected Ids Ids;
        protected IAppDbContext Context;
        protected readonly IMapper Mapper;
        protected IAuthorizedUserProvider UserProvider => _userProvider.Object;
        protected readonly CancellationToken CancellationToken = CancellationToken.None;

        public TestBase()
        {
            MapperConfiguration mapperConfig = new(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly)));
            Mapper = mapperConfig.CreateMapper();
            _userProvider = new();
        }

        protected void SetAuthorizedUserId(Guid id)
        {
            _userProvider.Setup(provider => provider.GetUserId()).Returns(id);
        }

        public void CreateDatabase()
        {
            Context = TestDbContextFactory.CreateFake(out Ids); //Can be turned to TestDbContextFactory.Create(out Ids); for real database instances
        }

        public void Dispose()
        {
            TestDbContextFactory.Destroy(Context);
        }
    }
}

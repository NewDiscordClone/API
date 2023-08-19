using Application.Interfaces;
using Application.Mapping;
using Application.Providers;
using AutoMapper;

namespace Tests.Common
{
    public class TestQueryBase
    {
        private readonly Mock<IAuthorizedUserProvider> _userProvider;

        protected readonly IAppDbContext Context;
        protected readonly IMapper Mapper;
        protected IAuthorizedUserProvider UserProvider => _userProvider.Object;
        protected readonly CancellationToken CancellationToken = CancellationToken.None;

        public TestQueryBase()
        {
            Context = TestDbContextFactory.Create();
            MapperConfiguration mapperConfig = new(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly)));
            Mapper = mapperConfig.CreateMapper();
            _userProvider = new();
            SetAuthorizedUserId(1);
        }

        protected void SetAuthorizedUserId(int id)
        {
            _userProvider.Setup(provider => provider.GetUserId()).Returns(id);
        }
    }
}

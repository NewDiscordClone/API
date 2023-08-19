using Application.Interfaces;
using Application.Mapping;
using AutoMapper;

namespace Tests.Common
{
    public class TestQueryBase
    {
        protected readonly IAppDbContext Context;
        protected readonly IMapper Mapper;
        protected readonly CancellationToken CancellationToken = CancellationToken.None;

        public TestQueryBase()
        {
            Context = TestDbContextFactory.Create();
            MapperConfiguration mapperConfig = new(config =>
                config.AddProfile(new AssemblyMappingProfile(
                    typeof(IAppDbContext).Assembly)));
            Mapper = mapperConfig.CreateMapper();
        }
    }
}

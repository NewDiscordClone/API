using AutoMapper;
using Sparkle.Domain.Common.Interfaces;

namespace Sparkle.Domain.LookUps
{
    public class RoleViewModel : IMapWith<Role>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Color { get; init; }
        public int Priority { get; init; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleViewModel>();
        }
    }
}

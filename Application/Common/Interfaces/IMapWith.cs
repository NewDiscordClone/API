using AutoMapper;

namespace Sparkle.Application.Common.Interfaces
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile);
    }
}

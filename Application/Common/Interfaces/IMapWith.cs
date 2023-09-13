using AutoMapper;

namespace Application.Common.Interfaces
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile);
    }
}

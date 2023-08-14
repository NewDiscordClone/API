using AutoMapper;

namespace Application.Interfaces
{
    public interface IMapWith<T>
    {
        void Mapping(Profile profile);
    }
}

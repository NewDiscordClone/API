using AutoMapper;

namespace Sparkle.Domain.Common.Interfaces
{
    /// <summary>
    /// Interface for mapping objects using AutoMapper.
    /// </summary>
    /// <typeparam name="T">The type to map with.</typeparam>
    public interface IMapWith<T>
    {
        /// <summary>
        /// Configures the mapping of the object using AutoMapper.
        /// </summary>
        /// <param name="profile">The AutoMapper profile to use for mapping.</param>
        void Mapping(Profile profile);
    }
}

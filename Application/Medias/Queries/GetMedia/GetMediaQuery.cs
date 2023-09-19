using MediatR;
using Sparkle.Application.Models;
using System.ComponentModel;

namespace Sparkle.Application.Medias.Queries.GetMedia
{
    public record GetMediaQuery : IRequest<Media>
    {
        /// <summary>
        /// Id of the media recourse
        /// </summary>
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; set; }
    }
}
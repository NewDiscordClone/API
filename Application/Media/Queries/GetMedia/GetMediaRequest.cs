using Application.Models;
using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Media.Queries.GetMedia
{
    public record GetMediaRequest : IRequest<Media>
    {
        /// <summary>
        /// Id of the media recourse
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; set; }
    }
}
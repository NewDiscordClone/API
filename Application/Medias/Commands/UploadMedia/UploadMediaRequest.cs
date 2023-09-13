using MediatR;
using Microsoft.AspNetCore.Http;
using Sparkle.Application.Models;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Medias.Commands.UploadMedia
{
    public class UploadMediaRequest : IRequest<Media>
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
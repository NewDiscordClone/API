using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Commands.UploadMedia
{
    public class UploadMediaRequest : IRequest<Media>
    {
        public IFormFile File { get; set; }
    }
}
using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Medias.Commands.UploadMedia
{
    public class UploadMediaCommandHandler(IAuthorizedUserProvider userProvider, IMediaRepository mediaRepository) :
        RequestHandler(userProvider), IRequestHandler<UploadMediaCommand, Media>
    {
        private readonly IMediaRepository _mediaRepository = mediaRepository;

        public async Task<Media> Handle(UploadMediaCommand command, CancellationToken cancellationToken)
        {
            using MemoryStream memoryStream = new();

            await command.File.CopyToAsync(memoryStream, cancellationToken);

            byte[] fileBytes = memoryStream.ToArray();
            Media media = new()
            {
                FileName = command.File.FileName,
                ContentType = command.File.ContentType,
                Data = fileBytes
            };
            return await _mediaRepository.AddAsync(media, cancellationToken);
        }
    }
}
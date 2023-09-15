using MediatR;
using MongoDB.Bson;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Medias.Commands.UploadMedia
{
    public class UploadMediaCommandHandler : RequestHandlerBase, IRequestHandler<UploadMediaCommand, Media>
    {
        public UploadMediaCommandHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }

        public async Task<Media> Handle(UploadMediaCommand command, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            using MemoryStream memoryStream = new();

            await command.File.CopyToAsync(memoryStream, cancellationToken);

            byte[] fileBytes = memoryStream.ToArray();
            string id = ObjectId.GenerateNewId().ToString();
            Media media = new()
            {
                Id = id,
                FileName = command.File.FileName,
                ContentType = command.File.ContentType,
                Data = fileBytes
            };
            return await Context.Media.AddAsync(media);
        }
    }
}
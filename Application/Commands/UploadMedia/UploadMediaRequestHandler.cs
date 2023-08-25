using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.UploadMedia
{
    public class UploadMediaRequestHandler : RequestHandlerBase, IRequestHandler<UploadMediaRequest, Media>
    {
        public UploadMediaRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }

        public async Task<Media> Handle(UploadMediaRequest request, CancellationToken cancellationToken)
        {
            using var memoryStream = new MemoryStream();

            await request.File.CopyToAsync(memoryStream, cancellationToken);

            var fileBytes = memoryStream.ToArray();
            var id = ObjectId.GenerateNewId();
            var media = new Media
            {
                Id = id,
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                Data = fileBytes,
                Extension = Path.GetExtension(request.File.FileName)[1..]
            };
            await Context.Media.InsertOneAsync(media, null, cancellationToken);
            return await Context.FindByIdAsync<Media>(id, cancellationToken);
        }
    }
}
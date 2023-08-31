using Application.Interfaces;
using Application.Models;
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
            Context.SetToken(cancellationToken);
            
            using var memoryStream = new MemoryStream();

            await request.File.CopyToAsync(memoryStream, cancellationToken);

            var fileBytes = memoryStream.ToArray();
            var id = ObjectId.GenerateNewId().ToString();
            var media = new Media
            {
                Id = id,
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                Data = fileBytes
            };
            return await Context.Media.AddAsync(media);
        }
    }
}
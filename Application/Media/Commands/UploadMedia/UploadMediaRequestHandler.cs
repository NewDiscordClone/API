using Application.Common.Interfaces;
using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Media.Commands.UploadMedia
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

            using MemoryStream memoryStream = new();

            await request.File.CopyToAsync(memoryStream, cancellationToken);

            byte[] fileBytes = memoryStream.ToArray();
            string id = ObjectId.GenerateNewId().ToString();
            Media media = new()
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
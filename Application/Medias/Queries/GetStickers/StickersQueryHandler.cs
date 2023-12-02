using MediatR;
using Microsoft.Extensions.Options;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Options;

namespace Sparkle.Application.Medias.Queries.GetStickers
{
    public class StickersQueryHandler : IRequestHandler<StickersQuery, string[]>
    {
        private readonly string _mediaUri;

        public Task<string[]> Handle(StickersQuery _, CancellationToken cancellationToken)
        {
            string[] stickerIds = Constants.Message.StickerIds;

            string[] stickerUris = new string[stickerIds.Length];

            for (int i = 0; i < stickerIds.Length; i++)
            {
                string stickerId = stickerIds[i];
                string stickerUri = $"{_mediaUri}/{stickerId}.png";

                stickerUris[i] = stickerUri;
            }

            return Task.FromResult(stickerUris);
        }

        public StickersQueryHandler(IOptions<ApiOptions> options)
        {
            string apiUrl = options.Value.ApiUri;
            _mediaUri = apiUrl + "/api/media";
        }
    }
}

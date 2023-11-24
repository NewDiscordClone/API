using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Common.Interfaces.Repositories;
using Sparkle.Application.Models;

namespace Sparkle.Application.Medias.Queries.GetMedia
{
    public class GetMediaQueryHandler(IAuthorizedUserProvider userProvider, IMediaRepository mediaRepository)
        : RequestHandlerBase(userProvider), IRequestHandler<GetMediaQuery, Media>
    {
        private readonly IMediaRepository _mediaRepository = mediaRepository;

        public async Task<Media> Handle(GetMediaQuery query, CancellationToken cancellationToken)
        {
            Media media = await _mediaRepository.FindAsync(query.Id);
            return /*media.Extension == request.Extension ? */media/*: 
            throw new EntityNotFoundException($"There is no Media file {request.Id}.{request.Extension}")*/;
        }
    }
}
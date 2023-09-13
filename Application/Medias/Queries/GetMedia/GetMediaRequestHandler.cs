using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Medias.Queries.GetMedia
{
    public class GetMediaRequestHandler : RequestHandlerBase, IRequestHandler<GetMediaRequest, Media>
    {
        public GetMediaRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }

        public async Task<Media> Handle(GetMediaRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Media media = await Context.Media.FindAsync(request.Id);
            return /*media.Extension == request.Extension ? */media/*: 
            throw new EntityNotFoundException($"There is no Media file {request.Id}.{request.Extension}")*/;
        }
    }
}
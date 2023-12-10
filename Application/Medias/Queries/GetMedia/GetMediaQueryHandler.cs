using MediatR;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Domain;

namespace Sparkle.Application.Medias.Queries.GetMedia
{
    public class GetMediaQueryHandler : RequestHandlerBase, IRequestHandler<GetMediaQuery, Media>
    {
        public GetMediaQueryHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context,
            userProvider)
        {
        }

        public async Task<Media> Handle(GetMediaQuery query, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Media media = await Context.Media.FindAsync(query.Id);
            return /*media.Extension == request.Extension ? */media/*: 
            throw new EntityNotFoundException($"There is no Media file {request.Id}.{request.Extension}")*/;
        }
    }
}
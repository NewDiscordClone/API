using System.Runtime.InteropServices.ComTypes;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using MongoDB.Driver;

namespace Application.Queries.GetMedia
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
            
            var media = await Context.Media.FindAsync(request.Id);
            return /*media.Extension == request.Extension ? */media/*: 
            throw new EntityNotFoundException($"There is no Media file {request.Id}.{request.Extension}")*/;
        }
    }
}
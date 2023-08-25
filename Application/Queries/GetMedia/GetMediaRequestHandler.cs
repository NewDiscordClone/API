using System.Runtime.InteropServices.ComTypes;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
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
            var media = await Context.FindByIdAsync<Media>(request.Id, cancellationToken);
            return /*media.Extension == request.Extension ? */media/*: 
            throw new EntityNotFoundException($"There is no Media file {request.Id}.{request.Extension}")*/;
        }
    }
}
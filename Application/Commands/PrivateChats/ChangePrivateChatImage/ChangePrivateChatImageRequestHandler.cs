using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.ChangePrivateChatImage
{
    public class ChangePrivateChatImageRequestHandler : RequestHandlerBase, IRequestHandler<ChangePrivateChatImageRequest>
    {
        public async Task Handle(ChangePrivateChatImageRequest request, CancellationToken cancellationToken)
        {
            PrivateChat chat =
                await Context.FindByIdAsync<PrivateChat>(request.ChatId, cancellationToken);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            var oldImage = chat.Image;
            
            await Context.PrivateChats.UpdateOneAsync(
                Context.GetIdFilter<PrivateChat>(chat.Id),
                Builders<PrivateChat>.Update.Set(c => c.Image, request.NewImage)
                , null,
                cancellationToken);
            if(oldImage != null)
                await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/')-1)..], cancellationToken);
        }

        public ChangePrivateChatImageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
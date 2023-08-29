using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;
using MongoDB.Driver;

namespace Application.Commands.PrivateChats.ChangePrivateChatImage
{
    public class ChangePrivateChatImageRequestHandler : RequestHandlerBase, IRequestHandler<ChangePrivateChatImageRequest, PrivateChat>
    {
        public async Task<PrivateChat> Handle(ChangePrivateChatImageRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);
            
            PrivateChat chat = await Context.PrivateChats.FindAsync(request.ChatId);
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");
            var oldImage = chat.Image;

            chat.Image = request.NewImage;
            
            chat = await Context.PrivateChats.UpdateAsync(chat);
            
            if(oldImage != null)
                await Context.CheckRemoveMedia(oldImage[(oldImage.LastIndexOf('/')-1)..]);

            return chat;
        }

        public ChangePrivateChatImageRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
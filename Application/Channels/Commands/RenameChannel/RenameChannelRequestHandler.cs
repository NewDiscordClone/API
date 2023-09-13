using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Channels.Commands.RenameChannel
{
    public class RenameChannelRequestHandler : RequestHandlerBase, IRequestHandler<RenameChannelRequest>
    {
        public async Task Handle(RenameChannelRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Channel chat = await Context.Channels.FindAsync(request.ChatId);

            //TODO: Перевірити що у юзера є відповідні права
            if (!chat.Users.Any(u => u == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            chat.Title = request.NewTitle;

            await Context.Channels.UpdateAsync(chat);
        }

        public RenameChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
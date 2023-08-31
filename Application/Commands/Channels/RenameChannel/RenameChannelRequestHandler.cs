using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.Channels.RenameChannel
{
    public class RenameChannelRequestHandler : RequestHandlerBase, IRequestHandler<RenameChannelRequest>
    {
        public async Task Handle(RenameChannelRequest request, CancellationToken cancellationToken)
        {
            Context.SetToken(cancellationToken);

            Channel chat = await Context.Channels.FindAsync(request.ChatId);

            //TODO: Перевірити що у юзера є відповідні права
            if (!chat.Users.Any(u => u.Id == UserId))
                throw new NoPermissionsException("User is not a member of the chat");

            chat.Title = request.NewTitle;

            await Context.Channels.UpdateAsync(chat);
        }

        public RenameChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
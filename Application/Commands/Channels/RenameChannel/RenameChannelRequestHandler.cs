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
            Channel chat =
                await Context.FindByIdAsync<Channel>(request.ChatId, cancellationToken);

            //TODO: Перевірити що у юзера є відповідні права
            if (chat.Users.Find(u => u.Id == UserId) == null)
                throw new NoPermissionsException("User is not a member of the chat");
            chat.Title = request.NewTitle;

            await Context.SaveChangesAsync(cancellationToken);
        }

        public RenameChannelRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
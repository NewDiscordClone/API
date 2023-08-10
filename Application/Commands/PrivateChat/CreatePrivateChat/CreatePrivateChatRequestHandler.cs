using Application.Commands.Server.CreateServer;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Providers;
using MediatR;

namespace Application.Commands.PrivateChat.CreatePrivateChat
{
    public class CreatePrivateChatRequestHandler : RequestHandlerBase,  IRequestHandler<CreatePrivateChatRequest, Models.PrivateChat>
    {

        public async Task<Models.PrivateChat> Handle(CreatePrivateChatRequest request, CancellationToken cancellationToken)
        {
            List<User> users = new();
            request.UsersId.ForEach(userId => users.Add(Context.Users.Find(userId)
                                                        ?? throw new EntityNotFoundException(
                                                            $"User {userId} not found")));
            User owner = await Context.FindByIdAsync<User>(UserId, cancellationToken);
            Models.PrivateChat privateChat = new()
            {
                Title = request.Title,
                Image = request.Image,
                Users = users,
                Messages = new List<Message>(),
                Owner = owner
            };

            await Context.PrivateChats.AddAsync(privateChat, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return privateChat;
        }

        public CreatePrivateChatRequestHandler(IAppDbContext context, IAuthorizedUserProvider userProvider) : base(context, userProvider)
        {
        }
    }
}
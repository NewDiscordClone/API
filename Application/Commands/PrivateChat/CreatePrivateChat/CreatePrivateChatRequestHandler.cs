using Application.Commands.Server.CreateServer;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Commands.PrivateChat.CreatePrivateChat
{
    public class CreatePrivateChatRequestHandler : IRequestHandler<CreatePrivateChatRequest, Models.PrivateChat>
    {
        private readonly IAppDbContext _context;

        public CreatePrivateChatRequestHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Models.PrivateChat> Handle(CreatePrivateChatRequest request, CancellationToken cancellationToken)
        {
            List<User> users = new();
            request.UsersId.ForEach(userId => users.Add(_context.Users.Find(userId)
                                                        ?? throw new EntityNotFoundException(
                                                            $"User {userId} not found")));
            Models.PrivateChat privateChat = new()
            {
                Title = request.Title,
                Image = request.Image,
                Users = users,
                Messages = new List<Message>()
            };

            await _context.PrivateChats.AddAsync(privateChat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return privateChat;
        }
    }
}
using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Commands.Messages.AddMessageRequest
{

    public class AddMessageRequestHandler : IRequestHandler<AddMessageRequest, Message>
    {
        private readonly IAppDbContext _context;

        public AddMessageRequestHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Message> Handle(AddMessageRequest request, CancellationToken cancellationToken)
        {
            Chat? chat = await _context.Chats
                 .FindAsync(new object[] { request.ChatId }, cancellationToken);
            User? user = await _context.Users.FindAsync(new object[] { request.UserId }, cancellationToken);

            if (chat is null || user is null)
                throw new Exception("Chat or user not found");

            Message message = new()
            {
                Text = request.Text,
                Chat = chat,
                SendTime = DateTime.UtcNow,
                User = user,
                Attachments = request.Attachments ?? new()
            };

            await _context.Messages.AddAsync(message, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return message;
        }
    }
}

using Application.Models;

namespace Application.Commands.Users.SendMessageToUser
{
    public class MessageChatDto
    {
        public string MessageId { get; init; }
        public string ChatId { get; init; }
    }
}
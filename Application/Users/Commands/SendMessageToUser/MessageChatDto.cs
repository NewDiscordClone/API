namespace Application.Users.Commands.SendMessageToUser
{
    public class MessageChatDto
    {
        /// <summary>
        /// The unique identifier of the sent message.
        /// </summary>
        public string MessageId { get; init; }

        /// <summary>
        /// The unique identifier of the chat where the message was sent.
        /// </summary>
        public string ChatId { get; init; }
    }
}
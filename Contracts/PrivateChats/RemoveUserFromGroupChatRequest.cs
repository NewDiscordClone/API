namespace Sparkle.Contracts.PrivateChats
{
    public record RemoveUserFromGroupChatRequest
    {
        /// <summary>
        /// The unique identifier of the member to be removed from the group chat.
        /// </summary>
        public Guid? ProfileId { get; init; }

        /// <summary>
        /// Indicates whether to remove the member silently without sending notifications. (Optional, default is false)
        /// </summary>
        public bool Silent { get; init; } = false;
    }
}

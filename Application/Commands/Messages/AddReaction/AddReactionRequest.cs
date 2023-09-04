namespace Application.Commands.Messages.AddReaction
{
    public record AddReactionRequest : IRequest<Reaction>
    {
        /// <summary>
        /// Id of the message to which to add a reaction
        /// </summary>
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string MessageId { get; init; }

        /// <summary>
        /// Emoji code
        /// </summary>
        [Required]
        [DefaultValue(":smile:")]
        public string Emoji { get; init; }
    }

}
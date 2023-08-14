using Application.Models;
using MediatR;

namespace Application.Commands.Messages.RemoveReaction
{
    public class RemoveReactionRequest : IRequest<Message>
    {
        public int ReactionId { get; init; }
    }
}
using Application.Models;
using MediatR;

namespace Application.Commands.Messages.RemoveReaction
{
    public class RemoveReactionRequest : IRequest
    {
        public int ReactionId { get; init; }
    }
}
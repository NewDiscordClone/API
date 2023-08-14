using Application.Models;
using MediatR;

namespace Application.Commands.Messages.AddReaction
{
    public class AddReactionRequest : IRequest<Reaction>
    {
        public int MessageId { get; init; }
        public string Emoji { get; init; }
    }
}
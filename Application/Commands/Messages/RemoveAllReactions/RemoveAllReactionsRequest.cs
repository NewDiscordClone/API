using Application.Models;
using MediatR;

namespace Application.Commands.Messages.RemoveAllReactions
{
    public class RemoveAllReactionsRequest : IRequest<Chat>
    {
        public int MessageId { get; init; }
    }
}
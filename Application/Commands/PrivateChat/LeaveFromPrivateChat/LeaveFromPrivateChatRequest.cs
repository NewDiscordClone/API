using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChat.LeaveFromPrivateChat
{
    public class LeaveFromPrivateChatRequest : IRequest
    {
        public int ChatId { get; init; }
    }
}
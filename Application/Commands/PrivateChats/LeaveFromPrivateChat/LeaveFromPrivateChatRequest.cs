using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChats.LeaveFromPrivateChat
{
    public class LeaveFromPrivateChatRequest : IRequest
    {
        public int ChatId { get; init; }
    }
}
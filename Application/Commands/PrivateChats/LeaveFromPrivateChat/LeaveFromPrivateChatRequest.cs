using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.LeaveFromPrivateChat
{
    public class LeaveFromPrivateChatRequest : IRequest
    {
        public string ChatId { get; init; }
    }
}
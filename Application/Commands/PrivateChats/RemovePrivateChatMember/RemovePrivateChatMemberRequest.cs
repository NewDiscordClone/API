using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
        public ObjectId MemberId { get; init; }
    }
}
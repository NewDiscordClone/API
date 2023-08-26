using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.RemovePrivateChatMember
{
    public class RemovePrivateChatMemberRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
        public int MemberId { get; init; }
    }
}
using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.AddMemberToPrivateChat
{
    public class AddMemberToPrivateChatRequest : IRequest
    {
        public ObjectId ChatId { get; init; }
        public ObjectId NewMemberId { get; init; }
    }
}
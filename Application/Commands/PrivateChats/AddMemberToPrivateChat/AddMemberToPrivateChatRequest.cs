using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.AddMemberToPrivateChat
{
    public class AddMemberToPrivateChatRequest : IRequest<PrivateChat>
    {
        public string ChatId { get; init; }
        public int NewMemberId { get; init; }
    }
}
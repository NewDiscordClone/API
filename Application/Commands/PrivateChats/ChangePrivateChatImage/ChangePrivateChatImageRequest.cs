using System.ComponentModel.DataAnnotations;
using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.ChangePrivateChatImage
{
    public class ChangePrivateChatImageRequest : IRequest<PrivateChat>
    {
        public string ChatId { get; init; }
        public string NewImage { get; init; }
    }
}
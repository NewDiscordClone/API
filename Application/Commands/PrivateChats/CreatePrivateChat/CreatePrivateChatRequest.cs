using System.ComponentModel.DataAnnotations;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.PrivateChats.CreatePrivateChat
{
    public class CreatePrivateChatRequest : IRequest<Models.PrivateChat>
    {
        [MaxLength(255)] 
        public string? Title { get; init; }

        [DataType(DataType.ImageUrl)] 
        public string? Image { get; init; }

        [MaxLength(11)]
        public List<ObjectId> UsersId { get; init; }
    }
}
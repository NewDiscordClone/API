using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Models;
using Application.Models.LookUps;
using MediatR;
using MongoDB.Bson;

namespace Application.Queries.GetMessages
{
    public record GetMessagesRequest : IRequest<List<MessageDto>>
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }
        
        /// <summary>
        /// The amount of messages that already loaded to skip them
        /// </summary>
        [Required]
        [DefaultValue(0)]
        public int MessagesCount { get; init; }
    }
}
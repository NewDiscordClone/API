using Application.Models;
using MediatR;
using MongoDB.Bson;

namespace Application.Commands.Messages.RemoveAttachment
{
    public class RemoveAttachmentRequest : IRequest<Chat>
    {
        public string MessageId { get; init; }
        public int AttachmentIndex { get; init; }
    }
}
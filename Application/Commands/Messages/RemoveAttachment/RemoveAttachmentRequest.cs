using Application.Models;
using MediatR;

namespace Application.Commands.Messages.RemoveAttachment
{
    public class RemoveAttachmentRequest : IRequest<Chat>
    {
        public int AttachmentId { get; init; }
    }
}
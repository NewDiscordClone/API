using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.RequestModels.GetMessages
{
    public record GetMessageLookUpDto : IMapWith<Message>
    {
        public int Id { get; init; }
        public GetMessageUserDto User { get; init; }
        public string Text { get; init; }
        public DateTime SendTime { get; init; }
        public List<GetAttachmentDto> Attachments { get; init; }
        public List<GetReactionDto> Reactions { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, GetMessageLookUpDto>();
        }
    }
}
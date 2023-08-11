using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Queries.GetPinnedMessages
{
    public record GetPinnedMessageLookUpDto : IMapWith<Message>
    {
        public int Id { get; init; }
        public GetPinnedMessageUserDto User { get; init; }
        public string Text { get; init; }
        public DateTime SendTime { get; init; }
        public List<GetPinnedMessageAttachmentDto> Attachments { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, GetPinnedMessageLookUpDto>();
        }
    }
}
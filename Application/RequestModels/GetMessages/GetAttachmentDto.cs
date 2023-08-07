using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.RequestModels.GetMessages
{
    public record GetAttachmentDto : IMapWith<Attachment>
    {
        public int Id { get; init; }
        public AttachmentType Type { get; init; }
        public string Path { get; init; }
        public bool IsSpoiler { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Attachment, GetAttachmentDto>();
        }
    }
}
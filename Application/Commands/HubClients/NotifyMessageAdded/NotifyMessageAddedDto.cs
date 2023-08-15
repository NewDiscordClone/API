using Application.Interfaces;
using Application.Models;
using AutoMapper;

namespace Application.Commands.NotifyClients.NotifyChatMembers
{
    public class NotifyMessageAddedDto : IMapWith<Message>
    {
        public int Id { get; init; }
        public string Text { get; init; }
        public int ChatId { get; init; }
        public List<NotifyMessageAttachmentDto> Attachments { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Message, NotifyMessageAddedDto>()
                .ForMember(m => m.ChatId,
                    opt =>
                        opt.MapFrom(m => m.Chat.Id));
        }
    }
}
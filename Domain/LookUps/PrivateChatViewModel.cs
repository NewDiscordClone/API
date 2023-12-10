using AutoMapper;
using Sparkle.Domain.Common.Interfaces;

namespace Sparkle.Domain.LookUps
{
    public class PrivateChatViewModel : IMapWith<PersonalChat>
    {
        public string Id { get; init; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public Guid? OwnerId { get; set; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public List<UserProfileViewModel> Profiles { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PersonalChat, PrivateChatViewModel>()
                .ForMember(dto => dto.Profiles, opt => opt.Ignore())
                .AfterMap((chat, model) =>
                {
                    if (chat is GroupChat groupChat)
                    {
                        if (groupChat.Title != null)
                        {
                            model.Title = groupChat.Title;
                        }
                        model.Image ??= groupChat.Image;
                    }
                });
        }
    }
}

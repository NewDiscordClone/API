using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models.LookUps
{
    public record PrivateChatLookUp : IMapWith<GroupChat>
    {
        /// <summary>
        /// The unique identifier of the private chat.
        /// </summary>
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; init; }

        /// <summary>
        /// The URL of the private chat's image.
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string Image { get; init; }

        /// <summary>
        /// The title of the private chat.
        /// </summary>
        [DefaultValue("Title")]
        public string Title { get; set; }

        /// <summary>
        /// The subtitle of the private chat with extra information such as users count for group chat or user's status for personal chat.
        /// </summary>
        [DefaultValue("4 members")]
        public string Subtitle { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupChat, PrivateChatLookUp>()
                .ForMember(dto => dto.Subtitle,
                    opt =>
                        opt.MapFrom(chat => chat.Users.Count + " members"))
                .ForMember(dto => dto.Title,
                    opt =>
                        opt.Condition((_, _, title) => !string.IsNullOrWhiteSpace(title)));
        }

        public PrivateChatLookUp() { }

        public PrivateChatLookUp(PersonalChat personalChat, UserLookUp other)
        {
            Id = personalChat.Id;
            Image = other.Avatar;
            Title = other.DisplayName;
            Subtitle = other.TextStatus;
        }
    }
}

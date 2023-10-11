using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.Queries.PrivateChatDetails
{
    public class PrivateChatViewModel : IMapWith<PersonalChat>
    {
        public string Id { get; init; }
        public string Title { get; set; }
        public string? Image { get; init; }
        public Guid? OwnerId { get; set; }
        public DateTime CreatedDate { get; init; }
        public DateTime UpdatedDate { get; init; }
        public List<UserProfileViewModel> Profiles { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PersonalChat, PrivateChatViewModel>()
                .ForMember(dto => dto.Profiles, opt => opt.Ignore());
        }
    }

    public class UserProfileViewModel : IMapWith<UserProfile>
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; set; }
        public string? AvatarUrl { get; set; }
        public string? TextStatus { get; set; }
        public UserStatus Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserProfile, UserProfileViewModel>();
        }
    }
}

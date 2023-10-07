using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using System.Runtime.Serialization;

namespace Sparkle.Application.Models.LookUps
{
    [KnownType(typeof(PersonalChatLookup))]
    [KnownType(typeof(GroupChatLookup))]
    public abstract class PrivateChatLookUp
    {
        /// <summary>
        /// The unique identifier of the private chat.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The title of the private chat.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The URL of the private chat's image.
        /// </summary>
        public string? Image { get; init; }
        public DateTime UpdatedDate { get; init; }
        public abstract void Mapping(Profile profile);
    }
    public class PersonalChatLookup : PrivateChatLookUp, IMapWith<User>
    {
        public UserStatus UserStatus { get; init; }
        public string UsersTextStatus { get; init; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<(User User, PersonalChat Chat), PersonalChatLookup>()
                .ForMember(dto => dto.UserStatus, opt => opt
                .MapFrom(src => src.User.Status))
                .ForMember(dto => dto.UsersTextStatus, opt => opt
                .MapFrom(src => src.User.TextStatus))
                .ForMember(dto => dto.Title, opt => opt
                .MapFrom(src => src.User.DisplayName ?? src.User.UserName))
                .ForMember(dto => dto.Image, opt => opt
                .MapFrom(src => src.User.Avatar))
                .ForMember(dto => dto.Id, opt => opt
                .MapFrom(src => src.Chat.Id))
                .ForMember(dto => dto.UpdatedDate, opt => opt
                .MapFrom(src => src.Chat.UpdatedDate));
        }
    }

    public class GroupChatLookup : PrivateChatLookUp, IMapWith<GroupChat>
    {
        /// <summary>
        /// The number of members in the private chat.
        /// </summary>
        public int MembersCount { get; init; }

        public override void Mapping(Profile profile)
        {
            profile.CreateMap<GroupChat, GroupChatLookup>()
                .ForMember(dto => dto.MembersCount, opt => opt
                .MapFrom(src => src.Profiles.Count));
        }
    }
}

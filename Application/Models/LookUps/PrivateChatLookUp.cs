namespace Sparkle.Application.Models.LookUps
{
    public record PrivateChatLookUp : IMapWith<GroupChat>
    {
        /// <summary>
        /// The unique identifier of the private chat.
        /// </summary>
        public string Id { get; init; }

        /// <summary>
        /// The URL of the private chat's image.
        /// </summary>
        public string? Image { get; init; }

        /// <summary>
        /// The title of the private chat.
        /// </summary>
        public string? Title { get; set; }
        public DateTime UpdatedDate { get; init; }

        /// <summary>
        /// The number of members in the private chat.
        /// </summary>
        public int MembersCount { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupChat, PrivateChatLookUp>()
                .ForMember(dto => dto.MembersCount, opt => opt
                .MapFrom(chat => chat.Profiles.Count))
                .ForMember(dto => dto.Title, opt => opt
                .Condition((_, _, title) => !string.IsNullOrWhiteSpace(title)));
        }

        public PrivateChatLookUp() { }

        public PrivateChatLookUp(PersonalChat personalChat, User other)
        {
            Id = personalChat.Id;
            Image = other.Avatar;
            UpdatedDate = personalChat.UpdatedDate;
            Title = other.DisplayName ?? other.UserName;
            MembersCount = personalChat.Profiles.Count;
        }
    }
}

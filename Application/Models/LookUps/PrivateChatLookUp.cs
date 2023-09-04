using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using Application.Models;
using Application.Queries.GetMessages;
using AutoMapper;
using MongoDB.Bson;

namespace Application.Models
{
    public record PrivateChatLookUp : IMapWith<GroupChat>
    {
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string Id { get; init; }
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string Image { get; init; }
        [DefaultValue("Title")]
        public string Title { get; init; }
        [DefaultValue("Subtitle")]
        public string Subtitle { get; init; }
        
        public List<UserLookUp> Users { get; init; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<GroupChat, PrivateChatLookUp>()
                .ForMember(dto => dto.Subtitle,
                    opt =>
                        opt.MapFrom(chat => chat.Users.Count + " members"));
        }
        public PrivateChatLookUp(){}

        public PrivateChatLookUp(PersonalChat personalChat, int userid)
        {
            UserLookUp other = personalChat.Users.First(u => u.Id != userid);
            Id = personalChat.Id;
            Users = personalChat.Users;
            Image = other.Avatar;
            Title = other.DisplayName;
            Subtitle = other.TextStatus;
        }
    }
}
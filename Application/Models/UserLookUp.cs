using Application.Interfaces;
using AutoMapper;
using MongoDB.Bson;

namespace Application.Models
{
    public class UserLookUp : IMapWith<User>
    {
        public ObjectId Id { get; set; }

        public string DisplayName { get; set; }
        public string Avatar { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLookUp>()
                .ForMember(ul => ul.DisplayName,
                    opt =>
                        opt.MapFrom(u => 
                            string.IsNullOrWhiteSpace(u.DisplayName) ? u.UserName : u.DisplayName));
        }
    }
}
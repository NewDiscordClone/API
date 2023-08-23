using Application.Interfaces;
using AutoMapper;

namespace Application.Models
{
    public class UserLookUp : IMapWith<User>
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
        //public byte[] Avatar { get; set; }

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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using AutoMapper;

namespace Application.Models
{
    public class UserLookUp : IMapWith<User>
    {
        [DefaultValue(1)]
        public int Id { get; set; }
        [DefaultValue("𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string DisplayName { get; set; }
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
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
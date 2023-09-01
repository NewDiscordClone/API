using Application.Interfaces;
using AutoMapper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    /// <summary>
    /// User dto for collections
    /// </summary>
    public class UserLookUp : IMapWith<User>
    {
        [DefaultValue(1)]
        public int Id { get; set; }

        /// <summary>
        /// Non-unique user name
        /// </summary>
        [StringLength(32, MinimumLength = 1)]
        [DefaultValue("𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Avatar url
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [RegularExpression("https?:\\/\\/(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\/api\\/media\\/[a-z0-9]{24}$")]
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
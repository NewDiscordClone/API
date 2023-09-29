using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models.LookUps
{
    /// <summary>
    /// User dto for collections
    /// </summary>
    public class UserLookUp : IMapWith<User>
    {
        public Guid Id { get; set; }

        /// <summary>
        /// Non-unique user name
        /// </summary>
        [DefaultValue("𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string DisplayName { get; set; }

        /// <summary>
        /// Avatar url
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [RegularExpression(@"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\/api\/media\/[a-z0-9]{24}$")]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string Avatar { get; set; }
        [DefaultValue("I am good")]
        public string TextStatus { get; set; }

        public UserStatus Status { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<User, UserLookUp>()
                .ForMember(ul => ul.DisplayName,
                    opt =>
                        opt.MapFrom(u => u.DisplayName ?? u.UserName));
        }
    }
}
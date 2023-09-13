using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.Models
{
    public class GroupChat : PersonalChat
    {
        public Guid OwnerId { get; set; }

        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; set; }
        [DefaultValue("Title")]
        public string? Title { get; set; }
    }
}
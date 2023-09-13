using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.GroupChats.Commands.CreateGroupChat
{
    public record CreateGroupChatRequest : IRequest<string>
    {
        /// <summary>
        /// The title of the group chat.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string? Title { get; init; }

        /// <summary>
        /// The URL of the image for the group chat. (Optional)
        /// </summary>
        [DataType(DataType.ImageUrl)]
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }

        /// <summary>
        /// The list of unique identifiers of users to be added to the group chat.
        /// </summary>
        [Required]
        [MaxLength(11)]
        public List<Guid> UsersId { get; init; }
    }
}

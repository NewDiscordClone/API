using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Application.Commands.PrivateChats.CreatePrivateChat
{
    public class CreatePrivateChatRequest : IRequest<string>
    {
        [Required]
        [MaxLength(100)] 
        public string? Title { get; init; }

        [DataType(DataType.ImageUrl)] 
        [DefaultValue("https://localhost:7060/api/media/5f95a3c3d0ddad0017ea9291")]
        public string? Image { get; init; }

        [Required]
        [MaxLength(11)]
        public List<int> UsersId { get; init; }
    }
}
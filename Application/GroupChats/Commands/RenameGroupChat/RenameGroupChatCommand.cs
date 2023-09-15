using MediatR;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sparkle.Application.GroupChats.Commands.RenameGroupChat
{
    public class RenameGroupChatCommand : IRequest
    {
        [Required]
        [StringLength(24, MinimumLength = 24)]
        [DefaultValue("5f95a3c3d0ddad0017ea9291")]
        public string ChatId { get; init; }

        [Required]
        [DefaultValue("NewTitle")]
        public string NewTitle { get; init; }
    }
}
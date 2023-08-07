#nullable enable
using Application.Models;

namespace Application.RequestModels.GetUser
{
    public class GetUserDto
    {
        public int Id { get; init; }
        public string DisplayName{ get; init; }
        public string Username{ get; init; }
        public string AvatarPath{ get; init; }
        public UserStatus Status { get; init; } = UserStatus.Online;
        public string? TextStatus{ get; init; }
    }
}
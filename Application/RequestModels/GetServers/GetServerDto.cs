#nullable enable
using Application.Models;

namespace Application.RequestModels.GetServer
{
    public class GetServerDto
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string? Image{ get; init; }
        public List<GetChannelDto> Channels{ get; init; }
        public List<GetServerProfileDto> ServerProfiles { get; init; } 
        public List<GetRoleDto> Roles{ get; init; }
    }
}
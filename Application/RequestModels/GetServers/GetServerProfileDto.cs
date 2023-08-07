using Application.RequestModels.GetUser;

namespace Application.RequestModels.GetServer
{
    public class GetServerProfileDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public virtual GetUserDto User { get; set; }
        public virtual List<GetRoleDto> Roles { get; set; }
    }
}
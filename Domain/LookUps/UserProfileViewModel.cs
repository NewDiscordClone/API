using AutoMapper;
using Sparkle.Domain.Common.Interfaces;

namespace Sparkle.Domain.LookUps
{
    public class UserProfileViewModel : IMapWith<UserProfile>
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string? AvatarUrl { get; set; }
        public string? TextStatus { get; set; }
        public UserStatus Status { get; set; }
        public string Name { get; set; }
        public virtual void Mapping(Profile profile)
        {
            profile.CreateMap<UserProfile, UserProfileViewModel>();
        }
    }

    public class ServerProfileViewModel : UserProfileViewModel, IMapWith<(ServerProfile Profile, User User)>
    {
        public List<RoleViewModel> Roles { get; init; }
        public override void Mapping(Profile profile)
        {
            profile.CreateMap<(ServerProfile Profile, User User), ServerProfileViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Profile.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Profile.UserId))
                .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Profile.DisplayName ?? src.User.DisplayName ?? src.User.UserName))
                .ForMember(dest => dest.TextStatus, opt => opt.MapFrom(src => src.User.TextStatus))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.Avatar))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.User.Status))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Profile.Roles));
        }
    }
    public class ServerProfileLookup : UserProfileViewModel, IMapWith<(ServerProfile Profile, User User)>
    {
        public RoleViewModel MainRole { get; init; }
        public override void Mapping(Profile profile)
        {
            profile.CreateMap<(ServerProfile Profile, User User), ServerProfileLookup>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Profile.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Profile.UserId))
                .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Profile.DisplayName ?? src.User.DisplayName ?? src.User.UserName))
                .ForMember(dest => dest.TextStatus, opt => opt.MapFrom(src => src.User.TextStatus))
                .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.Avatar))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.User.Status))
                .ForMember(dest => dest.MainRole, opt => opt.MapFrom(src => src.Profile.Roles.FirstOrDefault()));
        }
    }
}

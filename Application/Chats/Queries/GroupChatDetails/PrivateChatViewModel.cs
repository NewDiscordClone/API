﻿using AutoMapper;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;

namespace Sparkle.Application.Chats.Queries.GroupChatDetails
{
    public class PrivateChatViewModel : IMapWith<PersonalChat>
    {
        public string Id { get; init; }
        public string Title { get; set; }
        public string? Image { get; init; }
        public Guid? OwnerId { get; set; }
        public List<UserProfileViewModel> Profiles { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PersonalChat, PrivateChatViewModel>()
                .ForMember(dto => dto.Profiles, opt => opt.Ignore());
        }
    }

    public class UserProfileViewModel : IMapWith<UserProfile>
    {
        public Guid Id { get; init; }
        public Guid UserId { get; init; }
        public string Name { get; set; }
        public string? AvatarUrl { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserProfile, UserProfileViewModel>();
        }
    }
}

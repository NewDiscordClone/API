﻿using Application.Interfaces;
using Application.Models;
using AutoMapper;
using System.ComponentModel;

namespace Application.Queries.GetServerDetails
{
    public record ServerProfileLookupDto : IMapWith<ServerProfile>
    {
        /// <summary>
        /// Non-unique name of user on this server
        /// </summary>
        [DefaultValue("𝕾𝖊𝖗𝖛𝖊𝖗 𝕯𝖎𝖘𝖕𝖑𝖆𝖞𝕹𝖆𝖒𝖊")]
        public string? DisplayName { get; init; }

        /// <summary>
        /// Unique id of the user
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// Main role of the user
        /// </summary>
        public RoleDto? MainRole { get; init; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ServerProfile, ServerProfileLookupDto>()
                .ForMember(sp => sp.MainRole,
                    opt =>
                        opt.MapFrom(sp =>
                            sp.Roles.MaxBy(r => r.Priority)));
        }
    }
}
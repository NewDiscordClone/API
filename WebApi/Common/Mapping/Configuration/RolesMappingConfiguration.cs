﻿using Mapster;
using Sparkle.Application.Models;
using Sparkle.Application.Roles.Commands.Create;
using Sparkle.Application.Roles.Commands.Update;
using Sparkle.Contracts.Roles;

namespace Sparkle.WebApi.Common.Mapping.Configuration
{
    public class RolesMappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<(string ServerId, CreateRoleRequest Request), CreateRoleCommand>()
                .Map(dest => dest.ServerId, src => src.ServerId)
                .Map(dest => dest, src => src.Request);

            config.NewConfig<(string RoleId, UpdateRoleRequest Request), UpdateRoleCommand>()
                .Map(dest => dest.Id, src => src.RoleId)
                .Map(dest => dest, src => src.Request);

            config.NewConfig<Role, RoleResponse>();
        }
    }
}
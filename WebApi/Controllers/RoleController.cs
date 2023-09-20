using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Roles.Commands.ChangeColor;
using Sparkle.Application.Roles.Commands.ChangeName;
using Sparkle.Application.Roles.Commands.ChangePriority;
using Sparkle.Application.Roles.Commands.Create;
using Sparkle.Application.Roles.Commands.Delete;
using Sparkle.Application.Roles.Commands.Update;
using Sparkle.Application.Roles.Commands.UpdateClaims;
using Sparkle.Contracts.Roles;
using System.Security.Claims;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/servers/{serverId}/roles")]
    public class RoleController : ApiControllerBase
    {
        public RoleController(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper) : base(mediator, userProvider, mapper)
        {
        }

        /// <summary>
        /// Creates new role
        /// </summary>
        /// <param name="serverId">Id of the server to create the role</param>
        /// <param name="request">Role details</param>
        [HttpPost]
        public async Task<ActionResult> CreateRole(string serverId, CreateRoleRequest request)
        {
            CreateRoleCommand command = Mapper.Map<CreateRoleCommand>((serverId, request));
            Role role = await Mediator.Send(command);

            RoleResponse response = Mapper.Map<RoleResponse>(role);
            return Created("", response);
        }

        /// <summary>
        /// Deletes the role
        /// </summary>
        /// <param name="roleId">Id of the role to delete</param>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="404">Not Found. The role is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{roleId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> DeleteRole(Guid roleId)
        {
            await Mediator.Send(new DeleteRoleCommand { RoleId = roleId });
            return NoContent();
        }

        [HttpPut("{roleId}")]
        public async Task<ActionResult> UpdateRole(Guid roleId, UpdateRoleRequest request)
        {
            UpdateRoleCommand command = Mapper.Map<UpdateRoleCommand>((roleId, request));
            Role role = await Mediator.Send(command);

            RoleResponse response = Mapper.Map<RoleResponse>(role);
            return Ok(response);
        }

        [HttpPatch("{roleId}")]
        public async Task<ActionResult> PartialUpdateRole(Guid roleId, string? name, string? color, int? priority)
        {
            List<IRequest<Role>> commands = new();

            if (name is not null)
                commands.Add(new ChangeRoleNameCommand { RoleId = roleId, Name = name });

            if (color is not null)
                commands.Add(new ChangeRoleColorCommand { RoleId = roleId, Color = color });

            if (priority is not null)
                commands.Add(new ChangeRolePriorityCommand { RoleId = roleId, Priority = priority.Value });

            if (commands.Count == 0)
                return Problem(statusCode: StatusCodes.Status400BadRequest, title: "No changes specified");

            Role? role = null;
            foreach (IRequest<Role> command in commands)
            {
                role = await Mediator.Send(command);
            }

            RoleResponse response = Mapper.Map<RoleResponse>(role!);
            return Ok(response);
        }

        [HttpPatch("{roleId}/claims")]
        public async Task<ActionResult> UpdateRoleClaims(Guid roleId, IEnumerable<Claim> claims)
        {
            await Mediator.Send(new UpdateRoleClaimsCommand { RoleId = roleId, Claims = claims });
            return NoContent();
        }
    }
}

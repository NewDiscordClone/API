using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Roles.Commands.ChangeColor;
using Sparkle.Application.Servers.Roles.Commands.ChangeName;
using Sparkle.Application.Servers.Roles.Commands.ChangePriority;
using Sparkle.Application.Servers.Roles.Commands.Create;
using Sparkle.Application.Servers.Roles.Commands.Delete;
using Sparkle.Application.Servers.Roles.Commands.Update;
using Sparkle.Application.Servers.Roles.Commands.UpdateClaims;
using Sparkle.Application.Servers.Roles.Queries.RoleDetails;
using Sparkle.Application.Servers.Roles.Queries.ServerRolesList;
using Sparkle.Contracts.Roles;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/servers/{serverId}/roles")]
    public class RoleController : ApiControllerBase
    {
        public RoleController(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper) : base(mediator, userProvider, mapper)
        {
        }

        /// <summary>
        /// Returns all roles of the server
        /// </summary>
        /// <param name="serverId">Id of the server to get the roles</param>
        /// <response code="200">OK. List of roles</response>
        /// <response code="404">Not Found. The server is not found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> GetRoles(string serverId)
        {
            List<Role> roles = await Mediator.Send(new ServerRolesListQuery { ServerId = serverId });
            IEnumerable<RoleResponse> response = roles.ConvertAll(Mapper.Map<RoleResponse>);
            return Ok(response);
        }

        /// <summary>
        /// Returns the role details
        /// </summary>
        /// <param name="roleId">Id of the role</param>
        /// <response code="200">OK. Role details</response>
        /// <response code="404">Not Found. The role is not found</response>
        [HttpGet("{roleId}")]
        public async Task<ActionResult> GetRole(Guid roleId)
        {
            Role role = await Mediator.Send(new RoleDetailsQuery { RoleId = roleId });
            RoleResponse response = Mapper.Map<RoleResponse>(role);
            return Ok(response);
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

            return CreatedAtAction(nameof(GetRole), new { serverId, roleId = role.Id }, role.Id);
        }

        /// <summary>
        /// Deletes the role
        /// </summary>
        /// <param name="roleId">Id of the role to delete</param>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="404">Not Found. The role is not found</response>
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

        /// <summary>
        /// Updates the role
        /// </summary>
        /// <param name="roleId">Id of the role to update</param>
        /// <param name="request">New role's data</param>
        /// <response code="204">Operation is successful.</response>
        /// <response code="404">Not Found. The role is not found</response>
        [HttpPut("{roleId}")]
        public async Task<ActionResult> UpdateRole(Guid roleId, UpdateRoleRequest request)
        {
            UpdateRoleCommand command = Mapper.Map<UpdateRoleCommand>((roleId, request));
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Update role's name.
        /// </summary>
        /// <param name="roleId">ID of the role to update.</param>
        /// <param name="name">New role's name.</param>
        /// <response code="204">Operation is successful.</response>
        /// <response code="400">Bad Request. Invalid input data.</response>
        /// <response code="404">Not Found. The role is not found.</response>
        [HttpPatch("{roleId}/name")]
        public async Task<ActionResult> UpdateRoleName(Guid roleId, string name)
        {
            await Mediator.Send(new ChangeRoleNameCommand { RoleId = roleId, Name = name });

            return NoContent();
        }

        /// <summary>
        /// Update role's color.
        /// </summary>
        /// <param name="roleId">ID of the role to update.</param>
        /// <param name="color">New role's color.</param>
        /// <response code="204">Operation is successful.</response>
        /// <response code="400">Bad Request. Invalid input data.</response>
        /// <response code="404">Not Found. The role is not found.</response>
        [HttpPatch("{roleId}/color")]
        public async Task<ActionResult> UpdateRoleColor(Guid roleId, string color)
        {
            await Mediator.Send(new ChangeRoleColorCommand { RoleId = roleId, Color = color });

            return NoContent();
        }

        /// <summary>
        /// Update role's priority.
        /// </summary>
        /// <param name="roleId">ID of the role to update.</param>
        /// <param name="priority">New role's priority.</param>
        /// <response code="204">Operation is successful.</response>
        /// <response code="400">Bad Request. Invalid input data.</response>
        /// <response code="404">Not Found. The role is not found.</response>
        [HttpPatch("{roleId}/priority")]
        public async Task<ActionResult> UpdateRolePriority(Guid roleId, int priority)
        {
            await Mediator.Send(new ChangeRolePriorityCommand { RoleId = roleId });
            return NoContent();
        }

        /// <summary>
        /// Updates the permissions of the role
        /// </summary>
        /// <param name="roleId">Id of the role to update claims</param>
        /// <param name="claims">New set of claims</param>
        /// <returns></returns>
        [HttpPatch("{roleId}/claims")]
        public async Task<ActionResult> UpdateRoleClaims(Guid roleId, List<ClaimRequest> claims)
        {
            List<IdentityRoleClaim<Guid>> identityClaims = claims
                .ConvertAll(claimRequest => Mapper.Map<IdentityRoleClaim<Guid>>((roleId, claimRequest)));

            await Mediator.Send(new UpdateRoleClaimsCommand { RoleId = roleId, Claims = identityClaims });

            return NoContent();
        }
    }
}

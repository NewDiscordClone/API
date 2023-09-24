using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Servers.Commands.BanUser;
using Sparkle.Application.Servers.Commands.ChangeServerProfileDisplayName;
using Sparkle.Application.Servers.Commands.ChangeServerProfileRoles;
using Sparkle.Application.Servers.Commands.KickUser;
using Sparkle.Application.Servers.Commands.UnbanUser;
using Sparkle.Contracts.Servers;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/servers/{serverId}/profiles")]
    public class ServerProfileController : ApiControllerBase
    {
        public ServerProfileController(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper) : base(mediator, userProvider, mapper)
        {
        }

        /// <summary>
        /// Removes User from the server users list. The User can come back if would have an invitation
        /// </summary>
        ///<param name="serverId">Id of the server to kick user from</param>
        ///<param name="profileId">Id of the user to kick from the server</param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. Your request is incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{profileId}/kick")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> KickUser(string serverId, Guid profileId)
        {
            KickUserCommand command = new() { ServerId = serverId, UserId = profileId };
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Removes User from the server users list and put him in a black list.
        /// The User can't come back even if it would have an invitation
        /// </summary>
        ///<param name="profileId">Id of the user to ban</param>
        ///<param name="serverId">Id of the server to ban user from</param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. Your request is incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{profileId}/ban")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> BanUser(string serverId, Guid profileId)
        {
            BanUserCommand command = new() { ServerId = serverId, UserId = profileId };
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Removes User from the server's black list. Now the user could return if it would have an invitation
        /// </summary>
        ///<param name="serverId">Id of the server to unban user from</param>
        ///<param name="profileId">Id of the user to unban</param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. Your request is incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPost("{profileId}/unban")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UnbanUser(string serverId, Guid profileId)
        {
            UnbanUserCommand command = new() { ServerId = serverId, UserId = profileId };
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Changes the Display name of the server profile
        /// </summary>
        ///<param name="profileId">Id of the user to change the display name</param>
        ///<param name="serverId">Id of the server to change the display name</param>
        ///<param name="newName">New display name</param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. TYour request is incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPatch("{profileId}/name")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangeServerProfileDisplayName(
           string serverId,
            Guid profileId,
            string newName)
        {
            ChangeServerProfileDisplayNameCommand command = new()
            {
                ServerId = serverId,
                UserId = profileId,
                NewDisplayName = newName
            };
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Changes the set of roles of the give user
        /// </summary>
        /// <param name="serverId">Id of the server to change the roles</param>
        /// <param name="profileId">Id of the user to change the roles</param>
        /// <param name="request">New set of roles</param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. Your request is incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut("{profileId}/roles")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangeServerProfileRoles(string serverId, Guid profileId, UpdateServerProfileRolesRequest request)
        {
            UpdateServerProfileRolesCommand command = Mapper.Map<UpdateServerProfileRolesCommand>((serverId, profileId, request));
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
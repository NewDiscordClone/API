using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.ServerProfiles.Commands.BanUser;
using Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileDisplayName;
using Sparkle.Application.Servers.ServerProfiles.Commands.ChangeServerProfileRoles;
using Sparkle.Application.Servers.ServerProfiles.Commands.LeaveServer;
using Sparkle.Application.Servers.ServerProfiles.Commands.UnbanUser;
using Sparkle.Application.Servers.ServerProfiles.Queries.GetServerProfiles;
using Sparkle.Application.Servers.ServerProfiles.Queries.ServerProfileDetails;
using Sparkle.Contracts.Servers;
using Sparkle.WebApi.Attributes;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/servers/{serverId}/profiles")]
    public class ServerProfileController : ApiControllerBase
    {
        public ServerProfileController(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper) : base(mediator, userProvider, mapper)
        {
        }

        [HttpGet]
        public async Task<ActionResult> GetServerProfiles(string serverId)
        {
            ServerProfilesQuery query = new() { ServerId = serverId };
            List<ServerProfile> profiles = await Mediator.Send(query);

            return Ok(profiles.ConvertAll(Mapper.Map<ServerProfileLookupResponse>));
        }

        [HttpGet("{profileId}")]
        public async Task<ActionResult> GetServerProfile(Guid profileId)
        {
            ServerProfileDetailsQuery query = new() { ProfileId = profileId };
            ServerProfile profile = await Mediator.Send(query);

            return Ok(Mapper.Map<ServerProfileResponse>(profile));
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
        [ServerAuthorize(Claims = Constants.Claims.KickUsers)]
        public async Task<ActionResult> KickUser(string serverId, Guid profileId)
        {
            LeaveServerCommand command = new() { ServerId = serverId, ProfileId = profileId };
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
        [ServerAuthorize(Claims = Constants.Claims.BanUsers)]
        public async Task<ActionResult> BanUser(string serverId, Guid profileId)
        {
            BanUserCommand command = new() { ServerId = serverId, ProfileId = profileId };
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Leave the given server
        /// </summary>
        /// <param name="serverId">Id of the server to leave from</param>
        /// <param name="profileId">Id of the user's profile to leave the server</param>
        /// <returns>NoContent if the operation is successful</returns>
        /// <response code="204">NoContent. Successful operation</response>
        /// <response code="400">Bad Request. The server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpDelete("{profileId}/leave")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServerAuthorize]
        public async Task<ActionResult> LeaveServer(string serverId, Guid profileId)
        {
            await Mediator.Send(new LeaveServerCommand()
            {
                ServerId = serverId,
                ProfileId = profileId
            });

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
        [ServerAuthorize(Claims = Constants.Claims.BanUsers)]
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
        [ServerAuthorize(Policy = Constants.Policies.ChangeProfileName)]
        public async Task<ActionResult> ChangeServerProfileDisplayName(Guid profileId, string? newName)
        {
            ChangeServerProfileDisplayNameCommand command = new()
            {
                ProfileId = profileId,
                NewDisplayName = newName
            };
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// Changes the set of roles of the give user
        /// </summary>
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
        [ServerAuthorize(Policy = Constants.Claims.ManageRoles)]
        public async Task<ActionResult> ChangeServerProfileRoles(Guid profileId, UpdateServerProfileRolesRequest request)
        {
            UpdateServerProfileRolesCommand command = Mapper
                .Map<UpdateServerProfileRolesCommand>((profileId, request));
            await Mediator.Send(command);

            return NoContent();
        }
    }
}
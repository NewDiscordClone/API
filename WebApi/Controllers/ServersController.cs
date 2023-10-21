using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Constants;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.HubClients.Servers.ServerDeleted;
using Sparkle.Application.HubClients.Servers.ServerUpdated;
using Sparkle.Application.Models;
using Sparkle.Application.Servers.Commands.CreateServer;
using Sparkle.Application.Servers.Commands.DeleteServer;
using Sparkle.Application.Servers.Commands.JoinServer;
using Sparkle.Application.Servers.Commands.UpdateServer;
using Sparkle.Application.Servers.Queries.ServerDetails;
using Sparkle.Application.Servers.Queries.ServersList;
using Sparkle.Contracts.Servers;
using Sparkle.WebApi.Attributes;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/servers")]
    public class ServersController : ApiControllerBase
    {
        public ServersController(IMediator mediator, IAuthorizedUserProvider userProvider, IMapper mapper) : base(mediator, userProvider)
        {
            _mapper = mapper;
        }
        private readonly IMapper _mapper;

        /// <summary>
        /// Joins to the server via invitation
        /// </summary>
        /// <param name="invitationId">Id of the invitation to join to the server</param>
        /// <returns>NoContent if the operation is successful</returns>
        /// <response code="204">NoContent. Successful operation</response>
        /// <response code="400">Bad Request. The invitation is expired, not available or incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpPost("join/{invitationId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> JoinServer(string invitationId)
        {
            await Mediator.Send(new JoinServerCommand() { InvitationId = invitationId });

            return NoContent();
        }

        /// <summary>
        /// Gets all Servers the currently authorized user are member of
        /// </summary>
        /// <returns>List of the server look ups</returns>
        /// <response code="200">List of the server look ups</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<ServerLookUpDto>>> GetServers()
        {
            ServersListQuery get = new();
            List<ServerLookUpDto> servers = await Mediator.Send(get);

            return Ok(servers);
        }

        /// <summary>
        /// Gets the detailed information about the given server
        /// </summary>
        /// <param name="serverId">
        /// string ObjectId representation of a server to get details of
        /// </param>
        /// <returns>Server details</returns>
        /// <response code="200">Ok. Server details object in JSON</response>
        /// <response code="400">Bad Request. The requested server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpGet("{serverId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServerDetailsDto>> GetServerDetails(string serverId)
        {
            ServerDetailsQuery command = new() { ServerId = serverId };
            ServerDetailsDto server = await Mediator.Send(command);

            return Ok(server);
        }

        /// <summary>
        /// Creates new server
        /// </summary>
        /// <param name="command">
        /// ```
        /// title: string // up to 100 characters
        /// image?: string // URL to the image media file 
        /// ```
        /// </param>
        /// <returns>String ObjectId representation of newly created Server</returns>
        /// <response code="201">Created. String ObjectId representation of newly created Server</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CreateServer(CreateServerCommand command)
        {
            Server server = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetServerDetails), new { serverId = server.Id }, server.Id);
        }

        /// <summary>
        /// Changes the given server's title or image
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        /// <param name="serverId">Id of the server to update</param>
        /// <param name="request">
        /// ```
        /// title?: string // up to 100 characters
        /// image?: string // URL to the image media file
        /// ```
        /// </param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut("{serverId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ForbidResult))]
        [ServerAuthorize(Claims = Constants.Claims.ManageServer)]
        public async Task<ActionResult> UpdateServer(string serverId, UpdateServerRequest request)
        {
            UpdateServerCommand command = _mapper.Map<UpdateServerCommand>((request, serverId));

            await Mediator.Send(command);
            await Mediator.Send(new NotifyServerUpdatedQuery { ServerId = command.ServerId });

            return NoContent();
        }

        /// <summary>
        /// Deletes the server
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        ///<param name="serverId">Id of the server to delete</param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. Your request is incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete("{serverId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ServerAuthorize(Roles = Constants.Roles.ServerOwnerName)]
        public async Task<ActionResult> DeleteServer(string serverId)
        {
            DeleteServerCommand command = new() { ServerId = serverId };
            Server server = await Mediator.Send(command);

            await Mediator.Send(new NotifyServerDeletedQuery { Server = server });

            return NoContent();
        }
    }
}
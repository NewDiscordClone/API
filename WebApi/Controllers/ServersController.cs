using Application.Commands.Invitations.GetInvitationDetails;
using Application.Commands.Invitations.MakeInvitation;
using Application.Commands.Servers.BanUser;
using Application.Commands.Servers.CreateServer;
using Application.Commands.Servers.DeleteServer;
using Application.Commands.Servers.JoinServer;
using Application.Commands.Servers.KickUser;
using Application.Commands.Servers.LeaveServer;
using Application.Commands.Servers.UnbanUser;
using Application.Commands.Servers.UpdateServer;
using Application.Common;
using Application.Common.Exceptions;
using Application.Providers;
using Application.Queries.GetServer;
using Application.Queries.GetServerDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ServersController : ApiControllerBase
    {
        public ServersController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        { }


        /// <summary>
        /// Joins to the server via invitation
        /// </summary>
        /// <param name="id">Id of the invitation to join to the server</param>
        /// <returns>NoContent if the operation is successful</returns>
        /// <response code="204">NoContent. Successful operation</response>
        /// <response code="400">Bad Request. The invitation is expired, not available or incorrect</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [Route("{id}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> JoinServer(string id)
        {
            try
            {
                await Mediator.Send(new JoinServerRequest() { InvitationId = id });
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        /// <summary>
        /// Leave the given server
        /// </summary>
        /// <param name="id">Id of the server to leave from</param>
        /// <returns>NoContent if the operation is successful</returns>
        /// <response code="204">NoContent. Successful operation</response>
        /// <response code="400">Bad Request. The server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [Route("{id}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> LeaveServer(string id)
        {
            try
            {
                await Mediator.Send(new LeaveServerRequest() { ServerId = id });
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Gets all Servers the currently authorized user are member of
        /// </summary>
        /// <returns>List of the server look ups</returns>
        /// <response code="200">Ok. List of the server look ups</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<GetServerLookupDto>>> GetServers()
        {
            GetServersRequest get = new();
            List<GetServerLookupDto> servers = await Mediator.Send(get);
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServerDetailsDto>> GetServerDetails(string serverId)
        {
            try
            {
                ServerDetailsDto server = await Mediator
                    .Send(new GetServerDetailsRequest { ServerId = serverId });
                return Ok(server);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Creates new server
        /// </summary>
        /// <param name="request">
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
        public async Task<ActionResult<string>> CrateServer(CreateServerRequest request)
        {
            string id = await Mediator.Send(request);
            return Created($"{this.Request.Scheme}://{this.Request.Host}/api/GetServerDetails?=" + id, id);
        }

        /// <summary>
        /// Changes the given server's title or image
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        /// <param name="request">
        /// ```
        /// serverId: string // represents ObjectId of the server to edit
        /// title?: string // up to 100 characters
        /// image?: string // URL to the image media file
        /// ```
        /// </param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ForbidResult))]
        [ServerAuthorize(Policy = ServerClaims.ManageServer)]
        public async Task<ActionResult> UpdateServer(UpdateServerRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (NoPermissionsException ex)
            {
                return Forbid(ex.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Deletes the server
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        /// <param name="request">
        /// ```
        /// serverId: string // represents ObjectId of the server
        /// ```
        /// </param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ServerAuthorize(Policy = ServerClaims.ManageServer)]
        public async Task<ActionResult> DeleteServer(DeleteServerRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (NoPermissionsException ex)
            {
                return Forbid(ex.Message);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes User from the server users list. The User can come back if would have an invitation
        /// </summary>
        /// <param name="request">
        /// ```
        /// serverId: string // represents ObjectId of the server to kick user from
        /// userId: int // id of the user to kick from server
        /// ```
        /// </param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> KickUser(KickUserRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (NoPermissionsException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        /// <summary>
        /// Removes User from the server users list and put him in a black list.
        /// The User can't come back even if it would have an invitation
        /// </summary>
        /// <param name="request">
        /// ```
        /// serverId: string // represents ObjectId of the server to ban user from
        /// userId: int // id of the user to ban from server
        /// ```
        /// </param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> BanUser(BanUserRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (NoPermissionsException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        
        /// <summary>
        /// Removes User from the server's black list. Now the user could return if it would have an invitation
        /// </summary>
        /// <param name="request">
        /// ```
        /// serverId: string // represents ObjectId of the server to unban user from
        /// userId: int // id of the user to unban
        /// ```
        /// </param>
        /// <returns>No Content if the operation is successful</returns>
        /// <response code="204">No Content. Operation is successful</response>
        /// <response code="400">Bad Request. The requested server is not found</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        /// <response code="403">Forbidden. The client has not permissions to perform this action</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> UnbanUser(UnbanUserRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (NoPermissionsException)
            {
                return Forbid();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
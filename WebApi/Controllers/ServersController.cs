using Application.Commands.Servers.CreateServer;
using Application.Commands.Servers.DeleteServer;
using Application.Commands.Servers.UpdateServer;
using Application.Queries.GetServer;
using Application.Queries.GetServerDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using MongoDB.Bson;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ServersController : ApiControllerBase
    {
        public ServersController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {}

        /// <summary>
        /// Gets all Servers the currently authorized user are member of
        /// </summary>
        /// <returns>List of the server look ups</returns>
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServerDetailsDto>> GetServerDetails(string serverId)
        {
            ServerDetailsDto server = await Mediator
                .Send(new GetServerDetailsRequest { ServerId = serverId });
            return Ok(server);
        }

        /// <summary>
        /// Creates new server
        /// </summary>
        /// <param name="request"> Server creation model
        /// ```
        /// title: string // up to 100 characters
        /// image?: string // URL to the image media file 
        /// ```
        /// </param>
        /// <returns>string ObjectId representation of newly created Server</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CrateServer(CreateServerRequest request)
        {
            string id = await Mediator.Send(request);
            return Created(string.Empty, id);
        }

        /// <summary>
        /// Changes the given server's title or image
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        /// <param name="request"> Server update model
        /// ```
        /// serverId: string // represents ObjectId of the server to edit
        /// title?: string // up to 100 characters
        /// image?: string // URL to the image media file
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> UpdateServer(UpdateServerRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// Deletes the server
        /// </summary>
        /// <remarks>
        /// This action can only be performed by a server member with an appropriate role
        /// </remarks>
        /// <param name="request"> Server Id model
        /// ```
        /// serverId: string // represents ObjectId of the server
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteServer(DeleteServerRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

using Application.Commands.Servers.CreateServer;
using Application.Commands.Servers.DeleteServer;
using Application.Commands.Servers.UpdateServer;
using Application.Common;
using Application.Providers;
using Application.Queries.GetServer;
using Application.Queries.GetServerDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ServersController : ApiControllerBase
    {
        public ServersController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        { }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<GetServerLookupDto>>> GetServers()
        {
            GetServersRequest get = new();
            List<GetServerLookupDto> servers = await Mediator.Send(get);
            return Ok(servers);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ServerDetailsDto>> GetServerDetails(string serverId)
        {
            ServerDetailsDto server = await Mediator
                .Send(new GetServerDetailsRequest { ServerId = serverId });
            return Ok(server);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> CrateServer(CrateServerDto serverDto)
        {
            CreateServerRequest request = new()
            {
                Title = serverDto.Title,
                Image = serverDto.Image,
            };
            string id = await Mediator.Send(request);
            return Created(string.Empty, id);
        }

        /// <summary>
        /// Update server title or image
        /// </summary>
        /// 
        /// <param name="request">Request needed to update server</param>
        /// <response code="204">Update successful</response>
        /// <response code="401">You need to sign in to execute this command</response>
        /// <response code="400">You send wrong data</response>
        /// <response code="403">You have no permissions to do this</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(UnauthorizedResult))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ForbidResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BadRequestResult))]
        [ServerAuthorize(Policy = ServerClaims.ManageServer)]
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

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ServerAuthorize(Policy = ServerClaims.ManageServer)]
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

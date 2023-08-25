using Application.Commands.Servers.CreateServer;
using Application.Commands.Servers.DeleteServer;
using Application.Commands.Servers.UpdateServer;
using Application.Queries.GetServer;
using Application.Queries.GetServerDetails;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Providers;
using MongoDB.Bson;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ServersController : ApiControllerBase
    {
        public ServersController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {}

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
        public async Task<ActionResult<ServerDetailsDto>> GetServerDetails(ObjectId serverId)
        {
            ServerDetailsDto server = await Mediator
                .Send(new GetServerDetailsRequest { ServerId = serverId });
            return Ok(server);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> CrateServer(CrateServerDto serverDto)
        {
            CreateServerRequest request = new()
            {
                Title = serverDto.Title,
                Image = serverDto.Image,
            };
            ObjectId id = await Mediator.Send(request);
            return Created(string.Empty, id);
        }

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

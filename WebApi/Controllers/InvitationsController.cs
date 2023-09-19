using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Invitations.Commands.CreateInvitation;
using Sparkle.Application.Invitations.Queries.InvitationDetails;
using Sparkle.Contracts.Invitations;

namespace Sparkle.WebApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class InvitationsController : ApiControllerBase
    {
        public InvitationsController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {
        }

        /// <summary>
        /// Gets Invitation details
        /// </summary>
        /// <param name="id">
        /// Id of the invitation to get details from
        /// </param>
        /// <returns>Invitation details in JSON</returns>
        /// <response code="200">Ok. Invitation details in JSON</response>
        /// <response code="403">BadRequest. The invitation is expired, not available or incorrect</response>
        [HttpGet("invitations/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<InvitationDetailsDto>> GetInvitation(string id)
        {
            InvitationDetailsQuery query = new() { InvitationId = id };
            InvitationDetailsDto details = await Mediator.Send(query);

            return Ok(details);
        }

        /// <summary>
        /// Create an invitation as a link
        /// </summary>
        /// <param name="serverId">Id of the server to create an invitation for</param>
        /// <param name="request">
        /// ```
        /// includeUser: bool // Show the user that makes this invitation
        /// expireTime?: Date // Define when the invitation will be expired
        /// ```
        /// </param>
        /// <returns>Invitation details in JSON</returns>
        /// <response code="201">Created. InvitationLink</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        //TODO: Перевірити чи користувач має відповідні права
        [HttpPost("servers/{serverId}/invitations/create")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Invite(string serverId, CreateInvitationRequest request)
        {
            CreateInvitationCommand command = Mapper.Map<CreateInvitationCommand>((serverId, request));
            string id = await Mediator.Send(command);

            return CreatedAtAction(nameof(GetInvitation), id, id);
        }
    }
}
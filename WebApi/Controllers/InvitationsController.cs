using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Invitations.Commands.CreateInvitation;
using Sparkle.Application.Invitations.Queries.GetInvitationDetails;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/[controller]")]
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
        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<InvitationDetailsDto>> Invitation(string id)
        {
            try
            {
                GetInvitationDetailsRequest get = new();
                InvitationDetailsDto details = await Mediator.Send(get);
                return Ok(details);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
        /// <summary>
        /// Create an invitation as a link
        /// </summary>
        /// <param name="request">
        /// ```
        /// serverId: string // represents ObjectId of the server to invite to
        /// includeUser: bool // Show the user that makes this invitation
        /// expireTime?: Date // Define when the invitation will be expired
        /// ```
        /// </param>
        /// <returns>Invitation details in JSON</returns>
        /// <response code="201">Created. InvitationLink</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        //TODO: Перевірити чи користувач має відповідні права
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<string>> Invite(CreateInvitationRequest request)
        {
            string id = await Mediator.Send(request);
            return Created($"{Request.Scheme}://{Request.Host}/api/Invitation/" + id, id);
        }
    }
}
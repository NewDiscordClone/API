using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.PrivateChats.AddMemberToPrivateChat;
using Application.Commands.PrivateChats.ChangePrivateChatImage;
using Application.Commands.PrivateChats.CreatePrivateChat;
using Application.Commands.PrivateChats.LeaveFromPrivateChat;
using Application.Commands.PrivateChats.MakePrivateChatOwner;
using Application.Commands.PrivateChats.RemovePrivateChatMember;
using Application.Commands.PrivateChats.RenamePrivateChat;
using Application.Models;
using Application.Providers;
using Application.Queries.GetPrivateChatDetails;
using Application.Queries.GetPrivateChats;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PrivateChatsController : ApiControllerBase
    {
        public PrivateChatsController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(mediator, userProvider)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<GetPrivateChatLookUpDto>>> GetAllPrivateChats()
        {

            GetPrivateChatsRequest get = new() { UserId = UserId };
            List<GetPrivateChatLookUpDto> list = await Mediator.Send(get);
            return Ok(list);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetPrivateChatDetailsDto>> GetPrivateChatDetails(int chatId)
        {
            try
            {
                GetPrivateChatDetailsDto chat = await Mediator
                    .Send(new GetPrivateChatDetailsRequest() { ChatId = chatId });
                return Ok(chat);
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<int>> CreatePrivateChat(CreatePrivateChatRequest chatRequest)
        {
            PrivateChat chat = await Mediator.Send(chatRequest);
            //TODO: Реалізація відправки Notify
            return Created("https://localhost:7060/api/PrivateChat/GetDetails?chatId="+chat.Id, chat.Id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddMemberToPrivateChat(AddMemberToPrivateChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangePrivateChatImage(ChangePrivateChatImageRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }
        
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RenamePrivateChat(RenamePrivateChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> LeaveFromPrivateChat(LeaveFromPrivateChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoSuchUserException e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> MakePrivateChatOwner(MakePrivateChatOwnerRequest request)
        {
            try
            {
                await Mediator.Send(request);
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> RemovePrivateChatMember(RemovePrivateChatMemberRequest request)
        {
            try
            {
                await Mediator.Send(request);
                //TODO: Реалізація відправки Notify
                return Ok();
            }
            catch (NoPermissionsException e)
            {
                return Forbid(e.Message);
            }
            catch (NoSuchUserException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

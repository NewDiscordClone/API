using Application.Commands.Server.DeleteServer;
using Application.Commands.Server.UpdateServer;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Application.Commands.PrivateChat.AddMemberToPrivateChat;
using Application.Commands.PrivateChat.ChangePrivateChatImage;
using Application.Commands.PrivateChat.CreatePrivateChat;
using Application.Commands.PrivateChat.LeaveFromPrivateChat;
using Application.Commands.PrivateChat.MakePrivateChatOwner;
using Application.Commands.PrivateChat.RemovePrivateChatMember;
using Application.Commands.PrivateChat.RenamePrivateChat;
using Application.Models;
using Application.Providers;
using Application.Queries.GetPrivateChatDetails;
using Application.Queries.GetPrivateChats;
using WebApi.Models;

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
        public async Task<ActionResult<List<GetPrivateChatLookUpDto>>> GetAll()
        {

            GetPrivateChatsRequest get = new() { UserId = UserId };
            List<GetPrivateChatLookUpDto> list = await Mediator.Send(get);
            return Ok(list);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<GetPrivateChatDetailsDto>> GetDetails(int chatId)
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
        public async Task<ActionResult<int>> Create(CreatePrivateChatRequest chatRequest)
        {
            PrivateChat chat = await Mediator.Send(chatRequest);
            return Created("https://localhost:7060/api/PrivateChat/GetDetails?chatId="+chat.Id, chat.Id);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> AddMember(AddMemberToPrivateChatRequest request)
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> ChangeImage(ChangePrivateChatImageRequest request)
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Rename(RenamePrivateChatRequest request)
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
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult> Leave(LeaveFromPrivateChatRequest request)
        {
            try
            {
                await Mediator.Send(request);
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
        public async Task<ActionResult> MakeOwner(MakePrivateChatOwnerRequest request)
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
        public async Task<ActionResult> RemoveMember(RemovePrivateChatMemberRequest request)
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
            catch (NoSuchUserException ex)
            {
                return BadRequest(ex);
            }
        }
    }
}

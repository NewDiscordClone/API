using Application.Commands.UploadMedia;
using Application.Exceptions;
using Application.Interfaces;
using Application.Models;
using Application.Queries.GetMedia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ApiControllerBase
    {

        public MediaController(IMediator mediator, IAuthorizedUserProvider userProvider) : base(
            mediator,
            userProvider)
        {
        }

        [HttpGet]
        [Route("{id}")]//.{ext}
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Index([FromRoute] ObjectId id, [FromQuery] bool details = false)//, [FromRoute] string ext
        {
            try
            {
                Media media = await Mediator.Send(new GetMediaRequest() { Id = id});//, Extension = ext 
                return details? Ok(media): File(media.Data, media.ContentType);
            }
            catch (EntityNotFoundException e)
            {
                return BadRequest(e.Message);
            }
        }

        private const int MaxFileSizeMb = 5;
        [HttpPost("upload")]
        public async Task<ActionResult> Index([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            if (file.Length >= MaxFileSizeMb * 1024 * 1024)
                return BadRequest($"File is too big, please upload files less than {MaxFileSizeMb} MB");
            await Mediator.Send(new UploadMediaRequest { File = file });
            return Ok("File uploaded successfully.");
        }
    }
}
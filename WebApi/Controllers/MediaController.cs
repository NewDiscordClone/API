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

        /// <summary>
        /// Gets a media by it's id
        /// </summary>
        /// <param name="id">Unique id string that represents ObjectId</param>
        /// <param name="details">
        /// By default false.<b/>
        /// If set to true, the result would be Json detailed information of the media<b/>
        /// If set to false, the result would be media content (data in binary) showed accordingly to it's content type</param>
        /// <returns>
        /// By default returns the media content in binary and show it accordingly to it's content type <b/>
        /// If the details param is set to true, returns json with the detailed information about the media
        /// ```
        /// ```
        /// </returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Index([FromRoute] string id, [FromQuery] bool details = false)
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

        
        private const int _maxFileSizeMb = 5;
        /// <summary>
        /// Uploads the media file to the database
        /// </summary>
        /// <param name="file">File information provided from uploading form
        /// ```
        /// Length
        /// Filename
        /// ContentType
        /// BinaryData
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        [Authorize]
        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Index([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            if (file.Length >= _maxFileSizeMb * 1024 * 1024)
                return BadRequest($"File is too big, please upload files less than {_maxFileSizeMb} MB");
            await Mediator.Send(new UploadMediaRequest { File = file });
            return Ok("File uploaded successfully.");
        }
    }
}
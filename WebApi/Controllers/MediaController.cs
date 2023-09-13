using Application.Commands.UploadMedia;
using Application.Common.Exceptions;
using Application.Models;
using Application.Providers;
using Application.Queries.GetMedia;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Attributes;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ExceptionFilter]
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
        /// <para>By default false.</para>
        /// <para>If set to true, the result would be Json detailed information of the media</para>
        /// <para>If set to false, the result would be media content (data in binary) showed accordingly to it's content type</para></param>
        ///
        /// <returns>
        /// <para>By default returns the media content in binary and show it accordingly to it's content type </para>
        /// <para>If the details param is set to true, returns json with the detailed information about the media</para>
        /// </returns>
        ///
        /// <response code="200"><para>Ok.</para>
        /// <para>By default returns the media content in binary and show it accordingly to it's content type </para>
        /// <para>If the details param is set to true, returns json with the detailed information about the media</para>
        /// </response>
        /// <response code="400">Bad Request. The requested media is not found</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Index([FromRoute] string id, [FromQuery] bool details = false)
        {
            try
            {
                Media media = await Mediator.Send(new GetMediaRequest() { Id = id });//, Extension = ext 
                return details ? Ok(media) : File(media.Data, media.ContentType);
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
        /// <param name="file">
        /// ```
        /// Length
        /// Filename
        /// ContentType
        /// BinaryData
        /// ```
        /// </param>
        /// <returns>Ok if the operation is successful</returns>
        /// <response code="201">Created. Operation is successful</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [Authorize]
        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Index([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");
            if (file.Length >= _maxFileSizeMb * 1024 * 1024)
                return BadRequest($"File is too big, please upload files less than {_maxFileSizeMb} MB");
            Media media = await Mediator.Send(new UploadMediaRequest { File = file });
            return Created($"{Request.Scheme}://{Request.Host}/api/Media/" + media.Id, "Operation is successful");
        }
    }
}
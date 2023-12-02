using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sparkle.Application.Common.Interfaces;
using Sparkle.Application.Medias.Commands.UploadMedia;
using Sparkle.Application.Medias.Queries.GetMedia;
using Sparkle.Application.Medias.Queries.GetStickers;
using Sparkle.Application.Models;

namespace Sparkle.WebApi.Controllers
{
    [Route("api/media")]
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
        [HttpGet("{id}.{ext}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult> Get(string id, bool details = false)
        {
            Media media = await Mediator.Send(new GetMediaQuery() { Id = id });

            return details ? Ok(media) : File(media.Data, media.ContentType);
        }

        /// <summary>
        /// Returns stickers uris
        /// </summary>
        /// <responce code="200">Stickers media uris in format api/media/objectId.png</responce>
        [HttpGet("stickers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<ActionResult<string[]>> GetStickers()
        {
            string[] stickerUris = await Mediator.Send(new StickersQuery());

            return Ok(stickerUris);
        }


        private const int _maxFileSizeMb = 10;
        /// <summary>
        /// Uploads media files to the database
        /// </summary>
        /// <param name="file">
        /// ```
        /// [
        ///     Length
        ///     Filename
        ///     ContentType
        ///     BinaryData
        /// ]
        /// ```
        /// </param>
        /// <returns>A list of file paths </returns>
        /// <response code="200">Ok. Returns a list of file paths</response>
        /// <response code="401">Unauthorized. The client must be authorized to send this request</response>
        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<string>>> Upload(List<IFormFile> file)
        {
            if (file == null || file.Count == 0)
                return BadRequest("No file uploaded.");

            List<string> paths = new();

            foreach (IFormFile formFile in file)
            {
                if (formFile.Length == 0)
                    continue; // Skip empty file

                if (formFile.Length >= _maxFileSizeMb * 1024 * 1024)
                    return BadRequest($"File '{formFile.FileName}' is too big, please upload file less than {_maxFileSizeMb} MB");

                Media media = await Mediator.Send(new UploadMediaCommand() { File = formFile });

                paths.Add($"{Request.Scheme}://{Request.Host}/api/media/" + media.Id + Path.GetExtension(media.FileName));
            }
            return Ok(paths);
        }
    }
}
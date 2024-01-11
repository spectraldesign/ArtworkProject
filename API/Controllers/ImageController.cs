using Application.DTO.Image;
using Application.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// API endpoint for Images
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/image")]
    public class ImageController : BaseApiController
    {
        /// <summary>
        /// Get all images.
        /// </summary>
        /// <returns>A list of all images, each with fields {Id, FileData, CreatorID, CreatorUsername, Likes, Views}</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<GetAllImagesDTO>>> getAllImagesQuery()
        {
            var result = await Mediator.Send(new GetAllImagesQuery());
            return Ok(result);
        }

        /// <summary>
        /// Get an image by its Id.
        /// </summary>
        /// <param name="id">Unique (string) identifier for an image</param>
        /// <returns>An image with fields {Id, FileData, CreatorID, CreatorUsername, Likes, Views}</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ImageDTO>>> getImageById(string id)
        {
            var result = await Mediator.Send(new GetImageQuery(id));
            return Ok(result);
        }
    }
}

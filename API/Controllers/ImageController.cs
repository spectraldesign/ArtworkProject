using Application.Commands.ImageCommands;
using Application.DTO.Image;
using Application.Queries.ImageQueries;
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
        /// <returns>A list of all images, each with fields {Id, Description, CreatorID}</returns>
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
        /// <returns>An image with fields {Id, FileData, CreatorID, CreatorUsername, Likes, Views, Description}</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<List<ImageDTO>>> getImageById(string id)
        {
            var result = await Mediator.Send(new GetImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="imageDTO">Image data should be provided as {imageDataString, descriptionString}</param>
        /// <returns>Status code 201 on success</returns>
        [HttpPost]
        public async Task<ActionResult<int>> createNewImage([FromBody] CreateImageDTO imageDTO)
        {
            var result = await Mediator.Send(new CreateImageCommand(imageDTO));
            return result == 1 ? StatusCode(201) : new BadRequestObjectResult(result);
        }

        /// <summary>
        /// Deletes an image (if you are the owner)
        /// </summary>
        /// <param name="id">ID of image to delete</param>
        /// <returns>200 on success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<int>> deleteImage(string id)
        {
            var result = await Mediator.Send(new DeleteImageCommand(id));
            if (result == -1)
            {
                return Problem(
                    title: "Error, no such image",
                    detail: $"Poll with ID: {id} was not found",
                    statusCode: StatusCodes.Status404NotFound
                    );
            }
            if (result == -2)
            {
                return Problem(
                    title: "Error, permission denied",
                    detail: $"Logged in user did not have permission to delete image with ID: {id}",
                    statusCode: StatusCodes.Status403Forbidden
                    );
            }
            return Ok($"Poll with ID: {id} deleted");
        }
    }
}

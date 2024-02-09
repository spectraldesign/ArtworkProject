using Application;
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
        /// Get images by page. Page must be 1 or more, less than max pages. Min page size: 1, Max page size: 100.
        /// </summary>
        /// <returns>{data: [{data: {ImageData: [{ImageDTO}], CurrentPage: int, PageCount: int}, success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<ApiResponseType<GetImageByPageDTO>>> GetImagesByPage([FromQuery] int page = 1, [FromQuery] int size = 30)
        {
            if (size <= 0 || size > 100)
            {
                return new BadRequestObjectResult(new ApiResponseType<GetImageByPageDTO?>(null, false, "Page size must be more than 0 and less than 101"));
            }
            if (page <= 0)
            {
                return new BadRequestObjectResult(new ApiResponseType<GetImageByPageDTO?>(null, false, "Page number must be 0 or higher"));
            }

            GetImageByPageDTO result = await Mediator.Send(new GetImagesByPageQuery(page - 1, size));
            if (result.ImageDTOs.Count == 0)
            {
                return new BadRequestObjectResult(new ApiResponseType<GetImageByPageDTO>(result, false, "No images found"));
            }
            return new ApiResponseType<GetImageByPageDTO>(result, true, "Images fetched successfully");
        }

        /// <summary>
        /// Get an image by its Id.
        /// </summary>
        /// <param name="id">Unique (string) identifier for an image</param>
        /// <returns>{data: {LikeID, UserID, PollID}?, success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponseType<ImageDTO>>> GetImageById(string id)
        {
            var result = await Mediator.Send(new GetImageQuery(id));
            if (result == null)
            {
                return new BadRequestObjectResult(new ApiResponseType<ImageDTO?>(null, false, $"No image with Id: {id} found!"));
            }
            return new ApiResponseType<ImageDTO>(result, true);
        }

        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="imageDTO">Image data should be provided as {imageDataString, descriptionString}</param>
        /// <returns>{data: {data: int, success, message, responseCode}</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponseType<int>>> CreateNewImage([FromBody] CreateImageDTO imageDTO)
        {
            var result = await Mediator.Send(new CreateImageCommand(imageDTO));
            if (result != 1)
            {
                return new BadRequestObjectResult(new ApiResponseType<CreateImageDTO?>(null, false, "An error occured creating image"));
            }
            return new ApiResponseType<int>(result, true, "Image created successfully");
        }

        /// <summary>
        /// Deletes an image (if you are the owner)
        /// </summary>
        /// <param name="id">ID of image to delete</param>
        /// <returns>{data: int, success, message, responseCode}</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponseType<int>>> DeleteImage(string id)
        {
            var result = await Mediator.Send(new DeleteImageCommand(id));
            if (result == -1)
            {
                return new BadRequestObjectResult(new ApiResponseType<int>(result, false, $"Image with ID: {id} was not found"));
            }
            if (result == -2)
            {
                return new BadRequestObjectResult(new ApiResponseType<int>(result, false, $"Logged in user did not have permission to delete image with ID: {id}"));
            }
            return new ApiResponseType<int>(result, true, $"Image with ID: {id} deleted");
        }
    }
}

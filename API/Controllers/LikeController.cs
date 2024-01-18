using Application;
using Application.Commands.LikeCommands;
using Application.DTO.Like;
using Application.Queries.LikeQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Like API endpoint
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/like")]
    public class LikeController : BaseApiController
    {
        /// <summary>
        /// Gets a list of all likes present on an image
        /// </summary>
        /// <param name="imageId">The ID of the image to get likes for</param>
        /// <returns>{data: [{LikeID, UserID, PollID}], success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpGet("/image/{imageId}")]
        public async Task<ActionResult<ApiResponseType<List<LikeDTO>>>> GetLikesByImageId(string imageId)
        {
            var result = await Mediator.Send(new GetLikesByImageIdQuery(imageId));
            if (result.Count == 0)
            {
                return new BadRequestObjectResult(new ApiResponseType<List<LikeDTO>>([], false, $"No likes found on image with id {imageId}", 404));
            }
            return new ApiResponseType<List<LikeDTO>>(result, true);
        }

        /// <summary>
        /// Gets a list of all likes by a specific user
        /// </summary>
        /// <param name="userId">The ID of the user to get likes for</param>
        /// <returns>{data: [{LikeID, UserID, PollID}], success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpGet("/user/{userId}")]
        public async Task<ActionResult<ApiResponseType<List<LikeDTO>>>> GetLikesByUserId(string userId)
        {
            var result = await Mediator.Send(new GetLikesByUserIdQuery(userId));
            if (result.Count == 0)
            {
                return new BadRequestObjectResult(new ApiResponseType<List<LikeDTO>>([], false, $"No likes found by user with id {userId}", 404));
            }
            return new ApiResponseType<List<LikeDTO>>(result, true);
        }

        /// <summary>
        /// Create a new like on an image by its ID.
        /// </summary>
        /// <param name="imageId">The ID of the image to like.</param>
        /// <returns>{data: "", success, message, responseCode}</returns>
        [HttpPost("{imageId}")]
        public async Task<ActionResult<ApiResponseType<string>>> CreateLike(string imageId)
        {
            var result = await Mediator.Send(new CreateLikeCommand(imageId));
            if (result == -1)
            {
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, $"Image with ID: {imageId} was not found", 404));
            }
            return new ApiResponseType<string>("", true, "Like created successfully.", 201);
        }
    }
}

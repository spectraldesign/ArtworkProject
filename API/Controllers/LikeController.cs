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
        /// <returns>A list of {LikeID, UserID, PollID}</returns>
        [AllowAnonymous]
        [HttpGet("/image/{imageId}")]
        public async Task<ActionResult<List<LikeDTO>>> GetLikesByImageId(string imageId)
        {
            var result = await Mediator.Send(new GetLikesByImageIdQuery(imageId));
            return Ok(result);
        }

        /// <summary>
        /// Gets a list of all likes by a specific user
        /// </summary>
        /// <param name="userId">The ID of the user to get likes for</param>
        /// <returns>A list of {LikeID, UserID, PollID}</returns>
        [AllowAnonymous]
        [HttpGet("/user/{userId}")]
        public async Task<ActionResult<List<LikeDTO>>> GetLikesByUserId(string userId)
        {
            var result = await Mediator.Send(new GetLikesByUserIdQuery(userId));
            return Ok(result);
        }

        /// <summary>
        /// Create a new like on an image by its ID.
        /// </summary>
        /// <param name="imageId">The ID of the image to like.</param>
        /// <returns>201 on success</returns>
        [HttpPost("{imageId}")]
        public async Task<ActionResult<int>> CreateLike(string imageId)
        {
            var result = await Mediator.Send(new CreateLikeCommand(imageId));
            if (result == -1)
            {
                return Problem(
                    title: "Error, no such image",
                    detail: $"Image with ID: {imageId} was not found",
                    statusCode: StatusCodes.Status404NotFound
                    );
            }
            return StatusCode(201);
        }
    }
}

﻿using Application;
using Application.Commands.CommentCommands;
using Application.DTO.Comment;
using Application.Queries.CommentQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    /// <summary>
    /// Comment API endpoint
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/comment")]
    public class CommentController : BaseApiController
    {
        /// <summary>
        /// Get comments by an Image ID
        /// </summary>
        /// <param name="imageId">The ID of the image to get comments for</param>
        /// <returns>{data: [{Id, Content, UserId, ImageId, CreatedAt}], success, message, responseCode}</returns>

        [AllowAnonymous]
        [HttpGet("byimage/{imageId}")]
        public async Task<ActionResult<ApiResponseType<List<CommentDTO>>>> GetCommentsByImageId(string imageId)
        {
            var result = await Mediator.Send(new GetCommentsByImageIdQuery(imageId));
            if (result.Count == 0)
            {
                return new BadRequestObjectResult(new ApiResponseType<List<CommentDTO>>([], false, $"No comments found on image with id {imageId}"));
            }
            return new ApiResponseType<List<CommentDTO>>(result, true);
        }

        /// <summary>
        /// Get comments by a User ID
        /// </summary>
        /// <param name="userId">The ID of the user to get comments for</param>
        /// <returns>{data: [{Id, Content, UserId, ImageId, CreatedAt}], success, message, responseCode}</returns>
        [AllowAnonymous]
        [HttpGet("byuser/{userId}")]
        public async Task<ActionResult<ApiResponseType<List<CommentDTO>>>> GetCommentsByUserId(string userId)
        {
            var result = await Mediator.Send(new GetCommentsByUserIdQuery(userId));
            if (result.Count == 0)
            {
                return new BadRequestObjectResult(new ApiResponseType<List<CommentDTO>>([], false, $"No comments found by user with id {userId}"));
            }
            return new ApiResponseType<List<CommentDTO>>(result, true);
        }
        /// <summary>
        /// Create a new Comment (by the logged in user)
        /// </summary>
        /// <param name="commentDTO">{content: string, ImageId: valid string Guid for the image to comment on}</param>
        /// <returns>{data: "", success, message, responseCode}</returns>

        [HttpPost]
        public async Task<ActionResult<ApiResponseType<string>>> CreateComment([FromBody] CreateCommentDTO commentDTO)
        {
            var result = await Mediator.Send(new CreateCommentCommand(commentDTO));
            if (result == -1)
            {
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, $"Image with ID: {commentDTO.ImageId} was not found"));
            }
            if (result == -2)
            {
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, $"Comment cannot be empty!"));
            }
            return new ApiResponseType<string>("", true, "Comment created successfully.");
        }

        /// <summary>
        /// Delete a Comment (if you have permission)
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete</param>
        /// <returns>{data: "", success, message, responseCode}</returns>
        [HttpDelete("{commentId}")]
        public async Task<ActionResult<ApiResponseType<string>>> DeleteComment(string commentId)
        {
            var result = await Mediator.Send(new DeleteCommentCommand(commentId));
            if (result == -1)
            {
                return new BadRequestObjectResult(new ApiResponseType<string>("", false, $"Comment with ID: {commentId} was not found"));
            }
            if (result == -2)
            {
                return new BadRequestObjectResult(
                    new ApiResponseType<string>("", false, $"User does not have permission to delete comment with id: {commentId}!"));
            }
            return new ApiResponseType<string>("", true, "Comment deleted successfully");
        }

    }
}

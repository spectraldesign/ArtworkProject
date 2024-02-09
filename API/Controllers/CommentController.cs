using Application;
using Application.Commands.CommentCommands;
using Application.DTO.Comment;
using Application.Exceptions;
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
            ApiResponseType<List<CommentDTO>> response = new();
            if (result.Count == 0)
            {
                response.Success = false;
                response.Message = $"No comments found by user with id {userId}";
                return new BadRequestObjectResult(response);
            }
            response.Data = result;
            return new OkObjectResult(response);
        }
        
        /// <summary>
        /// Create a new Comment (by the logged in user)
        /// </summary>
        /// <param name="commentDTO">{content: string, ImageId: valid string Guid for the image to comment on}</param>
        /// <returns>{data: "", success, message, responseCode}</returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponseType<CommentDTO>>> CreateComment([FromBody] CreateCommentDTO commentDTO)
        {
            ApiResponseType<CommentDTO> response = new(); 
            try
            {
                var result = await Mediator.Send(new CreateCommentCommand(commentDTO));
                response.Data = result;
                response.Message = "Comment created successfully.";
                return response;
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return new BadRequestObjectResult(response);
            }
            
        }

        /// <summary>
        /// Delete a Comment (if you have permission)
        /// </summary>
        /// <param name="commentId">The ID of the comment to delete</param>
        /// <returns>{data: "", success, message, responseCode}</returns>
        [HttpDelete("{commentId}")]
        public async Task<ActionResult<ApiResponseType<CommentDTO>>> DeleteComment(string commentId)
        {
            ApiResponseType<CommentDTO> response = new();
            try
            {
                var result = await Mediator.Send(new DeleteCommentCommand(commentId));
                response.Data = result;
                response.Message = "Comment has successfully been deleted.";
                return response;
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

        }

    }
}

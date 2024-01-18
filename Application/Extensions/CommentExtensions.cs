using Application.DTO.Comment;
using Domain.Entities;

namespace Application.Extensions
{
    public static class CommentExtensions
    {
        public static CommentDTO ToCommentDTO(this Comment comment)
        {
            return new CommentDTO()
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ImageId = comment.CreatedAt.ToString(),
                UserId = comment.Creator.Id,
            };
        }

        public static Comment ToComment(this CreateCommentDTO commentDTO)
        {
            return new Comment()
            {
                Content = commentDTO.Content,
            };
        }
    }
}

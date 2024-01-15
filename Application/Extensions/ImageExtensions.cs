using Application.DTO.Image;
using Domain.Entities;

namespace Application.Extensions
{
    public static class ImageExtensions
    {
        public static ImageDTO ToImageDTO(this Image image)
        {
            return new ImageDTO()
            {
                Id = image.Id,
                FileData = image.FileData,
                CreatorId = image.Creator.Id,
                CreatorName = image.Creator.UserName,
                CommentCount = image.Comments.Count,
                LikeCount = image.Likes.Count,
                Views = image.Views,
                Description = image.Description
            };
        }

        public static Image ToImage(this CreateImageDTO imageDTO)
        {
            return new Image()
            {
                FileData = imageDTO.FileData,
            };
        }
    }
}

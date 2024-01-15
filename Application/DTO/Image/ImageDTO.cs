namespace Application.DTO.Image
{
    /// <summary>
    /// Data transfer object for Images. Contains all necessary info about an image.
    /// </summary>
    /// <param name="id">The unique ID of an Image</param>
    /// <param name="fileData">The image file data as a string</param>
    /// <param name="creator">The User who created/uploaded the Image</param>
    /// <param name="commentCount">Number of comments on the image</param>
    /// <param name="likeCount">Number of likes on the image</param>
    /// <param name="views">Number of views on the image</param>
    public class ImageDTO
    {
        public string Id { get; set; }
        public string FileData { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public int CommentCount { get; set; }
        public int LikeCount { get; set; }
        public int Views { get; set; }
        public string CreatedAt { get; set; }
        public string Description { get; set; }
    }
}

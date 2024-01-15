namespace Application.DTO.Image
{
    /// <summary>
    /// If fetching all images, only return the Id for the polls, further details can be requested by getting a specific poll by id.
    /// </summary>
    public class GetAllImagesDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
    }
}

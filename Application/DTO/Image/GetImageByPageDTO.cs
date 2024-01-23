namespace Application.DTO.Image
{
    public class GetImageByPageDTO
    {
        public List<ImageDTO> ImageDTOs { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}

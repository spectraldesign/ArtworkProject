using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Image
{
    public class CreateImageDTO
    {
        [Required]
        public string FileData { get; set; }
        public string Description { get; set; }
    }
}

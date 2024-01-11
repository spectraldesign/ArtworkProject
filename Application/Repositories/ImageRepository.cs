using Application.DTO.Image;
using Application.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Application.Repositories
{
    public interface IImageRepository
    {
        Task<ImageDTO> GetImageByIdAsync(string Id);
        Task<List<GetAllImagesDTO>> GetAllImagesAsync();
    }
    public class ImageRepository : IImageRepository
    {
        private readonly IGenericExtension _genericExtension;
        private readonly IArtworkProjectDbContext _artworkProjectDbContext;
        private readonly IConfiguration _configuration;
        //FOR TESTING, TODO: remove
        private readonly List<ImageDTO> _images;
        //FOR TESTING
        public ImageRepository(IGenericExtension genericExtension, IArtworkProjectDbContext artworkProjectDbContext, IConfiguration configuration)
        {
            _genericExtension = genericExtension;
            _artworkProjectDbContext = artworkProjectDbContext;
            _configuration = configuration;
            User user = new User()
            {
                UserName = "SomeUser",
            };
            User user2 = new User()
            {
                UserName = "SomeOtherUser"
            };
            ImageDTO image1 = new()
            {
                Id = "TestId1",
                FileData = "SomeSortOfBase64EncodedImageString",
                Creator = user,
                CommentCount = 4,
                LikeCount = 15,
                Views = 39,
            };
            ImageDTO image2 = new()
            {
                Id = "TestId2",
                FileData = "SomeOtherBase64EncodedImageString",
                Creator = user2,
                CommentCount = 441,
                LikeCount = 15525,
                Views = 391251251,
            };
            _images = new List<ImageDTO> { image1, image2 };
        }

        public async Task<List<GetAllImagesDTO>> GetAllImagesAsync()
        {
            //TODO: DB Request instead of this:
            List<GetAllImagesDTO> result = _images.Select(i => new GetAllImagesDTO() { Id = i.Id }).ToList(); //TODO: Use .ToListAsync when implemented DB
            return result;
        }

        public async Task<ImageDTO> GetImageByIdAsync(string Id)
        {
            ImageDTO result = _images.Where(i => i.Id == Id).Select(i => new ImageDTO()
            {
                Id = i.Id,
                Creator = i.Creator,
                LikeCount = i.LikeCount,
                CommentCount = i.CommentCount,
                FileData = i.FileData,
                Views = i.Views
            }).FirstOrDefault();
            return result;
        }
    }
}

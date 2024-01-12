using Application.DTO.Image;
using Application.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Repositories
{
    public interface IImageRepository
    {
        Task<ImageDTO> GetImageByIdAsync(string Id);
        Task<List<GetAllImagesDTO>> GetAllImagesAsync();
        Task<int> CreateImageAsync(CreateImageDTO createImageDTO);
    }
    public class ImageRepository : IImageRepository
    {
        private readonly IGenericExtension _genericExtension;
        private readonly IArtworkProjectDbContext _artworkProjectDbContext;
        private readonly IConfiguration _configuration;
        public ImageRepository(IGenericExtension genericExtension, IArtworkProjectDbContext artworkProjectDbContext, IConfiguration configuration)
        {
            _genericExtension = genericExtension;
            _artworkProjectDbContext = artworkProjectDbContext;
            _configuration = configuration;
        }

        public async Task<List<GetAllImagesDTO>> GetAllImagesAsync()
        {
            var response = await _artworkProjectDbContext.Images.Select(x => new GetAllImagesDTO { Id = x.Id }).ToListAsync();
            return response;
        }

        public async Task<ImageDTO> GetImageByIdAsync(string Id)
        {
            var response = await _artworkProjectDbContext.Images.Where(x => x.Id == Id).Select(x => new ImageDTO
            {
                Id = x.Id,
                FileData = x.FileData,
                CreatorId = x.Creator.Id,
                CreatorName = x.Creator.UserName,
                CommentCount = x.Comments.Count(),
                LikeCount = x.Likes.Count(),
                Views = x.Views,
            }).FirstAsync();
            return response;
        }

        public async Task<int> CreateImageAsync(CreateImageDTO createImageDTO)
        {
            User user = await _genericExtension.GetCurrentUserAsync();
            Image image = createImageDTO.ToImage();
            image.Id = Guid.NewGuid().ToString();
            image.Creator = user;
            image.Likes = new List<Like>();
            image.Comments = new List<Comment>();
            image.Views = 0;
            await _artworkProjectDbContext.Images.AddAsync(image);
            var saved = await _artworkProjectDbContext.SaveChangesAsync();
            return saved;
        }
    }
}


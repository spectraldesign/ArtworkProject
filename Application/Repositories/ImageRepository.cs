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
        Task<int> DeleteImageAsync(string Id);
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
            var response = await _artworkProjectDbContext.Images.Select(x => new GetAllImagesDTO
            {
                Id = x.Id,
                CreatorId = x.Creator.Id,
                Description = x.Description
            }).ToListAsync();
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
                CreatedAt = x.CreatedAt,
                Description = x.Description
            }).FirstOrDefaultAsync();
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
            image.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            image.Description = createImageDTO.Description;
            await _artworkProjectDbContext.Images.AddAsync(image);
            var saved = await _artworkProjectDbContext.SaveChangesAsync();
            return saved;
        }

        public async Task<int> DeleteImageAsync(string Id)
        {
            User currentUser = await _genericExtension.GetCurrentUserAsync();
            Image? dbImage = await _artworkProjectDbContext.Images.Where(x => x.Id == Id).FirstOrDefaultAsync();
            if (dbImage == null)
            {
                return -1;
            }
            string? dbImageCreatorId = await _artworkProjectDbContext.Images.Where(x => x.Id == Id).Select(x => x.Creator.Id).FirstOrDefaultAsync();
            if (dbImageCreatorId != currentUser.Id)
            {
                return -2;
            }
            _artworkProjectDbContext.Images.Remove(dbImage);
            int res = await _artworkProjectDbContext.SaveChangesAsync();
            return res;
        }
    }
}


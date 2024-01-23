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
        Task<GetImageByPageDTO> GetImagesByPageAsync(int page, int pageSize);
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

        public async Task<GetImageByPageDTO> GetImagesByPageAsync(int page, int pageSize)
        {
            List<Image> response = await _artworkProjectDbContext.Images.OrderByDescending(x => x.CreatedAt)
                .Include(x => x.Creator)
                .Include(x => x.Comments)
                .Include(x => x.Likes)
                .ToListAsync();
            List<ImageDTO> result = response.Skip(page * pageSize).Take(pageSize).ToList().Select(x => new ImageDTO()
            {
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                CreatorName = x.Creator.UserName,
                CreatorId = x.Creator.Id,
                CommentCount = x.Comments.Count,
                Description = x.Description,
                FileData = x.FileData,
                LikeCount = x.Likes.Count,
                Views = x.Views
            }).ToList();
            int pageCount = response.Count() / pageSize;

            return new GetImageByPageDTO() { ImageDTOs = result, PageCount = pageCount, CurrentPage = page };
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


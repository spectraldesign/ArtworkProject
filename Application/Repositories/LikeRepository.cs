using Application.DTO.Like;
using Application.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Repositories
{
    public interface ILikeRepository
    {
        Task<List<LikeDTO>> GetLikesByImageIdAsync(string pollId);
        Task<List<LikeDTO>> GetLikesByUserIdAsync(string userId);
        Task<int> CreateLikeAsync(string pollId);

    }
    public class LikeRepository : ILikeRepository
    {

        private readonly IArtworkProjectDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IGenericExtension _genericExtension;

        public LikeRepository(IGenericExtension genericExtension, IArtworkProjectDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = context;
            _genericExtension = genericExtension;
        }
        public async Task<List<LikeDTO>> GetLikesByImageIdAsync(string imageId)
        {
            var result = await _dbContext.Likes.Where(x => x.Image.Id == imageId).Select(x => new LikeDTO()
            {
                Id = x.Id,
                CreatorId = x.User.Id,
                ImageId = x.Image.Id
            }).ToListAsync();
            return result;
        }
        public async Task<List<LikeDTO>> GetLikesByUserIdAsync(string userId)
        {
            var result = await _dbContext.Likes.Where(x => x.User.Id == userId).Select(x => new LikeDTO()
            {
                Id = x.Id,
                CreatorId = x.User.Id,
                ImageId = x.Image.Id
            }).ToListAsync();
            return result;
        }

        public async Task<int> CreateLikeAsync(string imageId)
        {
            User currentUser = await _genericExtension.GetCurrentUserAsync();
            Image? image = await _dbContext.Images.Where(x => x.Id == imageId).FirstOrDefaultAsync();
            if (image == null)
            {
                return -1;
            }
            Like like = new Like()
            {
                Id = Guid.NewGuid().ToString(),
                User = currentUser,
                Image = image,
            };
            await _dbContext.Likes.AddAsync(like);
            var res = await _dbContext.SaveChangesAsync();
            return res;
        }
    }
}

using Application.DTO.Comment;
using Application.Extensions;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Repositories
{
    public interface ICommentRepository
    {
        Task<List<CommentDTO>> GetCommentsByImageIdAsync(string imageId);
        Task<List<CommentDTO>> GetCommentsByUserIdAsync(string userId);
        Task<int> CreateCommentCommandAsync(CreateCommentDTO commentDTO);
        Task<int> DeleteCommentCommandAsync(string commentId);
    }
    public class CommentRepository : ICommentRepository
    {
        private readonly IArtworkProjectDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IGenericExtension _genericExtension;

        public CommentRepository(IGenericExtension genericExtension, IArtworkProjectDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = context;
            _genericExtension = genericExtension;
        }

        public async Task<List<CommentDTO>> GetCommentsByImageIdAsync(string imageId)
        {
            var result = await _dbContext.Comments.Where(x => x.Image.Id == imageId).Select(x => new CommentDTO()
            {
                Id = x.Id,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                ImageId = x.Image.Id,
                UserId = x.Creator.Id
            }).ToListAsync();
            return result;
        }

        public async Task<List<CommentDTO>> GetCommentsByUserIdAsync(string userId)
        {
            var result = await _dbContext.Comments.Where(x => x.Creator.Id == userId).Select(x => new CommentDTO()
            {
                Id = x.Id,
                Content = x.Content,
                CreatedAt = x.CreatedAt,
                ImageId = x.Image.Id,
                UserId = x.Creator.Id
            }).ToListAsync();
            return result;
        }

        public async Task<int> CreateCommentCommandAsync(CreateCommentDTO commentDTO)
        {
            if (commentDTO.Content.Length == 0)
            {
                return -2;
            }
            User user = await _genericExtension.GetCurrentUserAsync();
            Image? image = await _dbContext.Images.Where(x => x.Id == commentDTO.ImageId).FirstOrDefaultAsync();
            if (image == null) { return -1; };
            Comment comment = commentDTO.ToComment();
            comment.Id = Guid.NewGuid().ToString();
            comment.Creator = user;
            comment.Image = image;
            comment.CreatedAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
            await _dbContext.Comments.AddAsync(comment);
            var saved = await _dbContext.SaveChangesAsync();
            return saved;
        }

        public async Task<int> DeleteCommentCommandAsync(string commentId)
        {
            Comment? comment = _dbContext.Comments.Where(x => x.Id == commentId).FirstOrDefault();
            if (comment == null) { return -1; }
            User currentUser = await _genericExtension.GetCurrentUserAsync();
            string creatorId = _dbContext.Comments.Where(x => x.Id == commentId).Select(x => x.Creator.Id).First();
            if (currentUser.Id != creatorId) { return -2; };
            _dbContext.Comments.Remove(comment);
            var res = await _dbContext.SaveChangesAsync();
            return res;
        }
    }
}

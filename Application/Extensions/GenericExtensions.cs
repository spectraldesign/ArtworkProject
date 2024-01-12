using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Extensions
{
    public interface IGenericExtension
    {
        Task<User> GetCurrentUserAsync();
    }
    public class GenericExtensions : IGenericExtension
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IArtworkProjectDbContext _context;
        public GenericExtensions(IHttpContextAccessor httpContextAccessor, IArtworkProjectDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<User> GetCurrentUserAsync()
        {
            string name = _httpContextAccessor.HttpContext.User.Identity.Name;
            User user = await _context.Users.Where(x => x.UserName == name).FirstOrDefaultAsync();
            return user;
        }
    }
}

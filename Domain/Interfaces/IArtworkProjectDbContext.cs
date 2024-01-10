using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Interfaces
{
    public interface IArtworkProjectDbContext
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}

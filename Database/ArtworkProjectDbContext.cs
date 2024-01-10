using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    public class ArtworkProjectDbContext : IdentityDbContext<User>, IArtworkProjectDbContext
    {
        public ArtworkProjectDbContext(DbContextOptions<ArtworkProjectDbContext> options) : base(options) { }
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

    }
}

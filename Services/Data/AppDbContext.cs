
using Microsoft.EntityFrameworkCore;
using Services.Entities;
using System.Collections.Generic;

namespace Services.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AvatarUrl> AvatarUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AvatarUrl>()
                .ToTable("images");
        }
    }
}

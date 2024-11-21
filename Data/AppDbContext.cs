using CrudForCategory.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudForCategory.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<CategoryMaster> CategoryMasters { get; set; }
        public DbSet<ProductMaster> ProductMasters { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductMaster>()
                .HasOne(p => p.Category)
                .WithMany(c => c.ProductMasters)
                .HasForeignKey(p => p.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

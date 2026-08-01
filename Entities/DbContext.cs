using Microsoft.EntityFrameworkCore;

namespace TaskManagementAPI.Entities
{
    public class DbContextEntity : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }
        public DbContextEntity(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; } 

    }
}

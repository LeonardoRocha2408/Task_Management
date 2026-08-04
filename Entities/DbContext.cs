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

            modelBuilder.Entity<TaskEntity>()
                .Property(t => t.Status)
                .HasConversion<string>();

            modelBuilder.Entity<TaskEntity>()
                .Property(t => t.Priority)
                .HasConversion<string>();
        }
        public DbContextEntity(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; } 
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
     }
}

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

            modelBuilder.Entity<ProjectsMembersEntity>()
                .Property(pm => pm.Role)
                .HasConversion<string>();

            modelBuilder.Entity<ProjectsMembersEntity>()
                .HasOne(pm => pm.Project)
                .WithMany()
                .HasForeignKey(pm => pm.ProjectId);

            modelBuilder.Entity<ProjectsMembersEntity>()
                .HasKey(pm => new
                {
                    pm.ProjectId,
                    pm.UserId
                });
        }
        public DbContextEntity(DbContextOptions options) : base(options) { }

        public DbSet<UserEntity> Users { get; set; } 
        public DbSet<ProjectEntity> Projects { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ProjectsMembersEntity> ProjectsMembers { get; set; }
     }
}

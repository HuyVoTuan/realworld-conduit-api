using Microsoft.EntityFrameworkCore;
using RealWorldConduit.Domain.Common;
using RealWorldConduit.Domain.Entities;

namespace RealWorldCondui.Infrastructure
{
    public class MainDbContext : DbContext
    {
        public static string UserSchema = "user";
        public static string BlogSchema = "blog";

        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<BlogTag> BlogsTag { get; set; }
        public DbSet<UserFollower> Followers { get; set; }
        public DbSet<FavoriteBlog> FavoriteBlogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditInfo()
        {
            var entities = ChangeTracker.Entries()
                                        .Where(e => e is IAuditEntity && (e.State == EntityState.Modified || e.State == EntityState.Added));

            foreach (var entity in entities)
            {
                ((IAuditEntity)entity.Entity).LastUpdatedAt = DateTime.UtcNow;

                if (entity.State is EntityState.Added)
                {
                    ((IAuditEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
                }
                else
                {
                    Entry((IAuditEntity)entity.Entity).Property(e => e.CreatedAt).IsModified = false;
                }
            }
        }
    }
}

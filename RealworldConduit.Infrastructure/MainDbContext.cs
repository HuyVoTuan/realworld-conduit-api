using RealWorldConduit.Domain.Common;

namespace RealWorldConduit.Infrastructure
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
        public DbSet<RefreshToken> RefreshTokens { get; set; }


        public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MainDbContext).Assembly);

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateAuditInfo();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateAuditInfo()
        {
            var entities = ChangeTracker.Entries()
                                        .Where(e => e.Entity is IAuditEntity && (e.State == EntityState.Modified || e.State == EntityState.Added));

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

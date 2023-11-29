namespace RealWorldConduit.Infrastructure.EntityConfigurations
{
    internal class FavoriteBlogConfiguration : IEntityTypeConfiguration<FavoriteBlog>
    {
        public void Configure(EntityTypeBuilder<FavoriteBlog> builder)
        {
            builder.ToTable<FavoriteBlog>(nameof(FavoriteBlog), MainDbContext.BlogSchema);

            builder.HasKey(x => new { x.BlogId, x.FavoritedById });

            builder.HasOne(x => x.Blog)
                   .WithMany(x => x.FavoriteBlogs)
                   .HasForeignKey(x => x.BlogId);

            builder.HasOne(x => x.FavoritedBy)
                   .WithMany(x => x.FavoriteBlogs)
                   .HasForeignKey(x => x.FavoritedById);
        }
    }
}

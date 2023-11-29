namespace RealWorldConduit.Infrastructure.EntityConfigurations
{
    internal class BlogTagConfiguration : IEntityTypeConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            builder.ToTable<BlogTag>(nameof(BlogTag), MainDbContext.BlogSchema);

            builder.HasKey(x => new { x.BlogId, x.TagId });

            builder.HasOne(x => x.Blog)
                   .WithMany(x => x.BlogTags)
                   .HasForeignKey(x => x.BlogId);

            builder.HasOne(x => x.Tag)
                   .WithMany(x => x.BlogTags)
                   .HasForeignKey(x => x.TagId);
        }
    }
}

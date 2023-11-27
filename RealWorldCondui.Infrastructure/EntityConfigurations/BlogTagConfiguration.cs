using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldCondui.Infrastructure.Extensions;
using RealWorldConduit.Domain.Entities;

namespace RealWorldCondui.Infrastructure.EntityConfigurations
{
    internal class BlogTagConfiguration : IEntityTypeConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            builder.ToTable<BlogTag>(nameof(BlogTag), MainDbContext.BlogSchema);

            builder.GenerateAudit<BlogTag>();

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

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldCondui.Infrastructure.Extensions;
using RealWorldConduit.Domain.Entities;

namespace RealWorldCondui.Infrastructure.EntityConfigurations
{
    internal class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.ToTable<Blog>(nameof(Blog), MainDbContext.BlogSchema);

            builder.GenerateId<Blog>();
            builder.GenerateAudit<Blog>();

            builder.Property(x => x.Title).HasMaxLength(150);

            builder.HasIndex(x => x.Title).IsUnique();

            builder.HasOne(x => x.Author)
                   .WithMany(x => x.Blogs)
                   .HasForeignKey(x => x.AuthorId);
        }
    }
}

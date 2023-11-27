using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldCondui.Infrastructure.Extensions;
using RealWorldConduit.Domain.Entities;

namespace RealWorldCondui.Infrastructure.EntityConfigurations
{
    internal class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable<Tag>(nameof(Tag), MainDbContext.BlogSchema);

            builder.GenerateId<Tag>();
            builder.GenerateAudit<Tag>();

            builder.Property(x => x.Name).HasMaxLength(50);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}

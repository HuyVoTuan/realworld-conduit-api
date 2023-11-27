using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldConduit.Domain.Entities;

namespace RealWorldCondui.Infrastructure.EntityConfigurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable<RefreshToken>(nameof(RefreshToken), MainDbContext.BlogSchema);

            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.AccessToken);
            builder.HasIndex(x => x.ExpiredDate);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.RefreshToken)
                   .HasForeignKey(x => x.UserId);
        }
    }
}

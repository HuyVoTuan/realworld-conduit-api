namespace RealWorldConduit.Infrastructure.EntityConfigurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable(nameof(RefreshToken), MainDbContext.UserSchema);

            builder.HasIndex(x => x.Token);
            builder.HasIndex(x => x.ExpiredDate);

            builder.Property(x => x.Token).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.RefreshToken)
                   .HasForeignKey(x => x.UserId);
        }
    }
}

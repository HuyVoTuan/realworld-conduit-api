namespace RealWorldConduit.Infrastructure.EntityConfigurations
{
    internal class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable<RefreshToken>(nameof(RefreshToken), MainDbContext.UserSchema);

            builder.HasIndex(x => x.AccessToken);
            builder.HasIndex(x => x.ExpiredDate);

            builder.Property(x => x.AccessToken).IsRequired();

            builder.HasOne(x => x.User)
                   .WithMany(x => x.RefreshToken)
                   .HasForeignKey(x => x.UserId);
        }
    }
}

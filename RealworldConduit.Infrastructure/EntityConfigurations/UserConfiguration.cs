namespace RealWorldConduit.Infrastructure.EntityConfigurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User), MainDbContext.UserSchema);

            builder.Property(u => u.Username).IsRequired().HasMaxLength(150);
            builder.Property(u => u.Password).IsRequired().HasMaxLength(250);
            builder.Property(u => u.Email).IsRequired().HasMaxLength(250);

            builder.HasIndex(x => x.Slug).IsUnique();
            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}

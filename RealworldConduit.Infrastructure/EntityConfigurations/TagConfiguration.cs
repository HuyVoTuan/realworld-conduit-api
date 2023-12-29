namespace RealWorldConduit.Infrastructure.EntityConfigurations
{
    internal class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.ToTable(nameof(Tag), MainDbContext.BlogSchema);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);

            builder.HasIndex(x => x.Name).IsUnique();
        }
    }
}

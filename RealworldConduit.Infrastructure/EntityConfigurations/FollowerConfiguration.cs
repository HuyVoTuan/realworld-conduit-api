namespace RealWorldConduit.Infrastructure.EntityConfigurations
{
    internal class FollowerConfiguration : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> builder)
        {
            builder.ToTable<UserFollower>(nameof(UserFollower), MainDbContext.UserSchema);

            builder.HasKey(x => new { x.FollowedUserId, x.FollowerId });

            builder.HasOne(x => x.FollowedUser)
                   .WithMany(x => x.FollowedUsers)
                   .HasForeignKey(x => x.FollowedUserId);
                   
            builder.HasOne(x => x.Follower)
                   .WithMany(x => x.Followers)
                   .HasForeignKey(x => x.FollowerId);
        }
    }
}

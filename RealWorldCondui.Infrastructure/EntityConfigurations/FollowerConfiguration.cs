using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealWorldCondui.Infrastructure.Extensions;
using RealWorldConduit.Domain.Entities;

namespace RealWorldCondui.Infrastructure.EntityConfigurations
{
    internal class FollowerConfiguration : IEntityTypeConfiguration<UserFollower>
    {
        public void Configure(EntityTypeBuilder<UserFollower> builder)
        {
            builder.ToTable<UserFollower>(nameof(UserFollower), MainDbContext.UserSchema);

            builder.GenerateAudit<UserFollower>();

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
